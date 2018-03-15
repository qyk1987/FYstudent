using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FYstudentMgr.Models;
using FYstudentMgr.ViewModels;
using FYstudentMgr.Helper;
using System.Web.Http.Cors;
using Microsoft.AspNet.Identity.Owin;
using FYstudentMgr.Common.Sms;
using FYstudentMgr.Manager;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    [RoutePrefix("api/Orders")]
    // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PostManager postmanager = new PostManager();
        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        public IQueryable<OrderWithProductIds> GetOrdersByStudent(int studentId)
        {
            return db.Orders.Where(o=>o.StudentID== studentId)
                .Select(o=>new OrderWithProductIds() {
                     Id=o.Id,
                     StudentID=o.StudentID,
                     ProductIds=o.OrderDetails.Select(od=>od.ProductId).ToList(),
                     OrderDate=o.OrderDate
                 });
        }

        /// <summary>
        /// 综合分类查询和搜索查询
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="state"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageResult<OrderResult>))]
        public async Task<IHttpActionResult> GetOrdersByPost(int postId,int state,string key,int pageSize,int page, string order, bool isAsc)
        {
            PageResult<OrderResult> result = new PageResult<OrderResult>();
            List<OrderResult> data = new List<OrderResult>();
            OrderResult ord = new OrderResult();
            var orders = db.Orders.Include(o => o.Compensations).Where(o => o.PostUserId == postId)
                .Where(o => state < 0 ? true : (int)o.State == state)
                .Where(o => o.State != OrderState.已删除)
                .Where(o => (key == "" || key == null) ? true : (o.OrderNO.IndexOf(key) > -1 ||
                       o.TradeNO.IndexOf(key) > -1 || o.Student.Name.IndexOf(key) > -1)
                        );
            
            var count = orders.Select(o => o.Id).Count();
            if (count == 0)
            {
                result.Count = 0;
                ord.orders = null;
                ord.statistic = null;
                data.Add(ord);
                result.Data = data;
                result.CurrentPage = 1;
                result.Order = order;
                result.IsAsc = isAsc;
                result.PageSize = pageSize;
                return Ok(result);
            }
            var os = isAsc ? LinqOrder.DataSort(orders, order, "asc") : LinqOrder.DataSort(orders, order, "desc");
            result.Count = count;
            ord.orders = await os.Skip((page - 1) * pageSize).Take(pageSize).Select(o => new OrderAllInfoWithProductIds()
            {
                Id = o.Id,
                StudentID = o.StudentID,
                TradeNO = o.TradeNO,
                OrderNO = o.OrderNO,
                PostUserId = o.PostUserId,
                OrderDate = o.OrderDate,
                ReceiptID = o.ReceiptID,
                ActualPay = o.OrderDetails.Sum(d => d.ActualPay),
                State = o.State,
                PayDate = o.PayDate,
                Channel = o.Channel,
                IsDebt = o.IsDebt,
                IsOtherDiscount = o.IsOtherDiscount,
                Debt = o.OrderDetails.Sum(d => d.Debt),
                Remark = o.Remark,
                Student = o.Student,
                ProductIds = o.OrderDetails.Select(od => od.ProductId).ToList(),
                Compensations = o.Compensations.ToList(),
                HasCompensation=o.Compensations.Count()>0
                }).ToListAsync();
            if (state == 1)
            {
                ord.statistic =await db.OrderDetails.Where(o => o.Order.PostUserId == postId)
                    .Where(o => (int)o.Order.State == state)
                    .GroupBy(o => o.Product.ProductName)
                    .Select(g => new StatiticDataItem()
                    {
                         ProductName=g.Key,
                         Count=g.Count(),
                          Debt=g.Sum(o=>o.Debt),
                           Discount=g.Sum(o=>o.Discount),
                           Pay=g.Sum(o=>o.ActualPay)
                    }).ToListAsync();
            }else
            {
                ord.statistic = null;
            }
            data.Add(ord);
            result.Data = data;
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result);
        }


        

        /// <summary>
        /// 搜索订单
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        //public IQueryable<OrderAllInfoWithProductIds> GetOrdersByPostAndKey(int postId,string key)
        //{
        //    return db.Orders.Where(o => o.PostUserId == postId)
        //        .Where(o=>o.OrderNO.IndexOf(key)>-1||
        //                o.TradeNO.IndexOf(key) > -1 || o.Student.Name.IndexOf(key) > -1
        //                )
        //        .Select(o => new OrderAllInfoWithProductIds()
        //        {
        //            Id = o.Id,
        //            StudentID = o.StudentID,
        //            TradeNO = o.TradeNO,
        //            OrderNO = o.OrderNO,
        //            PostUserId = o.PostUserId,
        //            OrderDate = o.OrderDate,
        //            ReceiptID = o.ReceiptID,
        //            ActualPay = o.OrderDetails.Sum(d => d.ActualPay),
        //            State = o.State,
        //            PayDate = o.PayDate,
        //            Channel = o.Channel,
        //            IsDebt = o.IsDebt,
        //            IsOtherDiscount = o.IsOtherDiscount,
        //            Debt = o.OrderDetails.Sum(d => d.Debt),
        //            Remark = o.Remark,
        //            Student = o.Student,
        //            ProductIds = o.OrderDetails.Select(od => od.ProductId).ToList()
        //        });
        //}

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }


        /// <summary>
        /// 申请结账功能实现  修改订单状态为待结账
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> GetOrderByUpdateState(int id, int State)
        {
            OrderState state = (OrderState)State;
            if (state != OrderState.待结账)
            {
                return BadRequest(ModelState);
            }
            var order = await db.Orders.FindAsync(id);
            
            if (order == null)
            {
                return BadRequest();
            }
            try
            {
                var receipt = db.Receipts.Where(r => r.State == ReceiptState.待结账).Where(o => o.PostUserId == order.PostUserId).SingleOrDefault();
                if (receipt == null)
                {
                    receipt = new Receipt();
                    receipt.CreateTime = DateTime.UtcNow;
                    receipt.State =ReceiptState.待结账;
                    receipt.PostUserId = order.PostUserId;
                    db.Receipts.Add(receipt);
                    order.ReceiptID = receipt.Id;
                }
                else
                {
                        order.ReceiptID = receipt.Id;
                }
                order.State = OrderState.待结账;
            
                db.Entry(order).State = EntityState.Modified;
                CheckOrderState(order.PostUserId);

                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            order.Receipt = null;//阻断循环引用
            return Ok(order);
        }
        // PUT: api/Orders/5
        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(order);
        }

     



        // POST: api/Orders
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> PostOrder(Order order)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Orders.Add(order);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = order.Id }, order);
        //}

        // POST: api/Orders
        [ResponseType(typeof(string))]
        public async Task<IHttpActionResult> PostOrder(Order[] orders)
        {
            //Order[] orders = Newtonsoft.Json.JsonConvert.DeserializeObject<Order[]>(ordersArray);
            if (orders.Length <= 0)
            {
                return NotFound();
            }
            try
            {
                var postId = orders.FirstOrDefault().PostUserId;
                var receipt = db.Receipts.Where(r => r.State == ReceiptState.未申请).Where(o => o.PostUserId == postId).SingleOrDefault();
                if (receipt == null)
                {
                    receipt = new Receipt();
                    receipt.CreateTime = DateTime.UtcNow;
                    receipt.State = ReceiptState.未申请;
                    receipt.PostUserId = postId;
                    db.Receipts.Add(receipt);
                }else
                {
                    receipt.ConfirmTime = null;
                    db.Entry(receipt).State = EntityState.Modified;
                }
                int i = 1;
                foreach (var order in orders)
                {

                    var ord = new Order()
                    {
                        Id = i,

                        Channel = order.Channel,
                        IsDebt = order.IsDebt,
                        IsOtherDiscount = order.IsOtherDiscount,
                        OrderDate = DateTime.UtcNow,
                         OrderNO = DateTime.Now.ToString("yyyyMMddHHmmss")+ new Random().Next(100, 999).ToString(),
                         PayDate = DateTime.UtcNow,
                         PostUserId = order.PostUserId,
                         Remark = order.Remark,
                         State =OrderState.待支付,
                         StudentID = order.StudentID,
                         TradeNO = order.TradeNO,
                         ReceiptID= receipt.Id

                    };
                    db.Orders.Add(ord);
                    foreach(var detail in order.OrderDetails)
                    {
                        detail.OrderId = ord.Id;
                        db.OrderDetails.Add(detail);
                    }
                    i++;
                }
                await db.SaveChangesAsync();
                return Ok("success");
            }catch(Exception e)
            {
                return  Ok(e.Message+e.StackTrace);
            }
            
            

        
        }

        [ResponseType(typeof(Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            try
            {
                order.State = OrderState.已删除;
                db.Entry(order).State = EntityState.Modified;
                CheckOrderState(order.PostUserId);
                // db.Orders.Remove(order);
                await db.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(order);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="productIds"></param>
        /// <param name="key"></param>
        /// <param name="pageSize"></param>
        /// <param name="page"></param>
        /// <param name="order"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        [Route("GetDetailsByDate")]
        [ResponseType(typeof(PageResult<OrderDetailVM> ))]
        public async Task<IHttpActionResult> GetDetailsByDate(int postid, string startDate, string endDate, string productIds, string key, int pageSize, int page, string order, bool isAsc)
        {
            PageResult<OrderDetailVM> result = new PageResult<OrderDetailVM>();
            int[] prdIds = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(productIds);
            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            var idlist = postmanager.GetSons(postid).Select(p => p.Id).ToList();
            key = key == "null" ? null : key;
            var details = db.OrderDetails.Include(od => od.Order.postUser.User)
                        .Include(od => od.Product)
                        .Include(od => od.Order.Student.School)
                .Where(od => idlist.Contains(od.Order.postUser.PostId))
                .Where(od => (key == "" || key == null) ? od.Order.OrderDate >= sdate:true)
                .Where(od => (key == "" || key == null) ? od.Order.OrderDate <= edate:true)
                .Where(od => (key == "" || key == null) ? prdIds.Contains(od.ProductId):true)
                .Where(od => (key == "" || key == null) ? true : (od.Order.Student.Name.IndexOf(key) > -1 || od.Order.Student.MobilePhoneNO.IndexOf(key) > -1));
            var count = details.Count();
            if (count == 0)
            {
                result.Data = null;
                result.Count = 0;
                result.CurrentPage = 1;
                result.Order = order;
                result.IsAsc = isAsc;
                result.PageSize = pageSize;
                return Ok(result);
            }
            result.Count = count;
            var ord = isAsc ? LinqOrder.DataSort(details, order, "asc") : LinqOrder.DataSort(details, order, "desc");
            var data = await ord.Skip((page - 1) * pageSize).Take(pageSize).Select(od => new OrderDetailVM()
                {
                    Id = od.Id,
                    ProductName=od.Product.ProductName,
                    SellerName=od.Order.postUser.User.Name,
                    StudentName=od.Order.Student.Name,
                    SchoolName=od.Order.Student.School.SchoolName,
                    ActualPay=od.ActualPay,
                    Discount=od.Discount,
                    Debt=od.Debt,
                    Date=od.Order.OrderDate
                }).ToListAsync();
            result.Data = data;
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result);

        }

        // DELETE: api/Orders/5
        //[ResponseType(typeof(Order))]
        //public async Task<IHttpActionResult> DeleteOrder(int id)
        //{
        //    Order order = await db.Orders.FindAsync(id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Orders.Remove(order);
        //    await db.SaveChangesAsync();

        //    return Ok(order);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                postmanager.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }

        private void CheckOrderState(int postId)
        {
            var orders = db.Orders.Where(o => o.State == OrderState.待支付).Where(o => o.PostUserId == postId);
            if (orders == null)
            {
                var receipts = db.Receipts.Where(r => r.State == ReceiptState.未申请).Where(r => r.PostUserId == postId);
                db.Receipts.RemoveRange(receipts);
            }
        }
    }
}