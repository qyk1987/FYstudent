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
using FYstudentMgr.Manager;
using FYstudentMgr.Helper;
using System.Web.Http.Cors;
using FYstudentMgr.Common.Sms;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    [RoutePrefix("api/Receipts")]
    // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ReceiptsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: api/Receipts
        public IQueryable<Receipt> GetReceipts()
        {
            return db.Receipts;
        }

        // GET: api/Receipts/5
        [ResponseType(typeof(ReceiptDetail))]
        public async Task<IHttpActionResult> GetReceipt(int id)
        {
            ReceiptDetail result = new ReceiptDetail(); 
            var orders =await db.Orders.Include(ord=>ord.Student)
                .Where(o=>o.State!=OrderState.已删除)
                .Where(o => o.ReceiptID == id)
                .ToListAsync();
            if (orders.Count() > 0)
            {
                List<OrderVMForAccount> details = new List<OrderVMForAccount>();
                foreach(var order in orders)
                {
                    var ds = db.OrderDetails.Where(d=>d.OrderId==order.Id).Include(d=>d.Product).Select(d => new OrderVMForAccount()
                    {
                         ActualPay=d.ActualPay,
                          Debt=d.Debt,
                          Discount=d.Discount,
                          ProductName=d.Product.ProductName,
                           OrderDate=order.OrderDate,
                            StudentName=order.Student.Name,
                             TradeNo=order.TradeNO,
                             Channel=order.Channel.ToString()
                    }).ToList();
                    details.AddRange(ds);
                }
                result.Orders = details;
            }
            var coms = db.Compensations.Include(c=>c.Order).Include(c=>c.Order.OrderDetails).Where(c => c.ReceiptID == id);
            if (coms.Count() > 0)
            {
                List<CompensationVMFoAccount> comdetails = new List<CompensationVMFoAccount>();
                comdetails = coms.Select(c => new CompensationVMFoAccount()
                {
                     Debt=c.Order.OrderDetails.Sum(d=>d.Debt),
                     ProductNames= c.Order.OrderDetails.Select(d=>d.Product.ProductName).ToList(),
                      StudentName=c.Order.Student.Name,
                       Value=c.Value,
                        FeeDate=c.CreateTime
                }).ToList();
                result.Fees = comdetails;
            }
            return Ok(result);
        }

        // GET: api/Receipts/5
        [ResponseType(typeof(ReceiptDetail))]
        public async Task<IHttpActionResult> GetReceiptID(int postid)
        {
            ReceiptDetail result = new ReceiptDetail();
            var orders = await db.Orders.Include(ord => ord.Student)
                    .Where(o => o.PostUserId== postid)
                    .Where(o=>o.State==OrderState.待结账).ToListAsync();
            if (orders.Count() > 0)
            {
                List<OrderVMForAccount> details = new List<OrderVMForAccount>();
                foreach (var order in orders)
                {
                    var ds = db.OrderDetails.Where(d => d.OrderId == order.Id).Include(d => d.Product).Select(d => new OrderVMForAccount()
                    {
                        ActualPay = d.ActualPay,
                        Debt = d.Debt,
                        Discount = d.Discount,
                        ProductName = d.Product.ProductName,
                        OrderDate = order.OrderDate,
                        StudentName = order.Student.Name,
                        TradeNo = order.TradeNO,
                        Channel = order.Channel.ToString()
                    }).ToList();
                    details.AddRange(ds);
                }
                result.Orders = details;
            }
         
                result.Fees = null;
            
            return Ok(result);
        }

        /// <summary>
        /// 业务员给财务发短信提醒
        /// </summary>
        /// <param name="puid">业务员岗位记录id</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        [Route("SetRemind")]
        public async Task<IHttpActionResult> GetRemind1(int puid)
        {
            var receipt = db.Receipts.Where(r => r.State == ReceiptState.待结账).Where(o => o.PostUserId == puid).SingleOrDefault();
            if (receipt == null)
            {
                return NotFound();
            }
            if (receipt.ConfirmTime!=null&& receipt.ConfirmTime.Value.Date >= DateTime.UtcNow.Date)
            {
                return NotFound();
            }
            receipt.ConfirmTime = DateTime.UtcNow;
            db.Entry(receipt).State = EntityState.Modified;
            var postuser = await db.PostUsers.FindAsync(puid);
            if (postuser == null)
            {
                return NotFound();
            }
            var post = db.Posts.Find(postuser.PostId);
            var senduser = db.Users.Find(post.UserId);
            var receivepost = db.Posts.Where(p => p.CampusId == post.CampusId).Where(p => p.Role.Label == "会计").FirstOrDefault();
            var receiveuser = db.Users.Find(receivepost.UserId);
            try
            {
                if (UserManager.SmsService != null)
                {
                    var message = new SmsMessage
                    {
                        Destination = receiveuser.PhoneNumber,
                        Body = "课程顾问:" + senduser.Name,
                        TemplateId = 32472
                    };
                    // message.setRecord(1, number);
                    // db.SmsRecords.Add(message.Record);
                    //await db.SaveChangesAsync();
                    await UserManager.SmsService.SendAsync(message);
                }
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(false);
        }

        /// <summary>
        /// 业务员获取今天是否已发短信
        /// </summary>
        /// <param name="puid"></param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        [Route("GetRemind")]
        public async Task<IHttpActionResult> GetRemind2(int puid)
        {
            var receipt = await db.Receipts.Where(r => r.State == ReceiptState.待结账).Where(o => o.PostUserId == puid).SingleOrDefaultAsync();
            if (receipt == null)
            {
                return NotFound();
            }
            if (receipt.ConfirmTime!=null&& receipt.ConfirmTime.Value.Date >= DateTime.UtcNow.Date)
            {
                return NotFound();
            }
            if (receipt.ConfirmTime == null)
            {
                return Ok(true);
            }
            if (receipt.ConfirmTime.Value.Date >= DateTime.UtcNow.Date)
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }


        }

        /// <summary>
        /// 财务给课程顾问发送结账提醒
        /// </summary>
        /// <param name="recid"></param>
        /// <returns></returns>
        [ResponseType(typeof(ReceiptVM))]
        [Route("RemindByAccount")]
        public async Task<IHttpActionResult> GetRemindByAccount(int recid)
        {
            var receipt = await db.Receipts.FindAsync(recid);
            if (receipt == null)
            {
                return NotFound();
            }
            if (receipt.ConfirmTime != null && receipt.ConfirmTime.Value.Date >= DateTime.UtcNow.Date)
            {
                return BadRequest();
            }
            receipt.ConfirmTime = DateTime.UtcNow;
            db.Entry(receipt).State = EntityState.Modified;
            var sid= User.Identity.GetUserId<string>();
            var senduser = db.Users.Find(sid);
            var recer = await db.PostUsers.FindAsync(receipt.PostUserId);
            var receiveuser = db.Users.Find(recer.UserId);
            try
            {
                if (UserManager.SmsService != null)
                {
                    var message = new SmsMessage
                    {
                        Destination = receiveuser.PhoneNumber,
                        Body = "公司财务:" + senduser.Name,
                        TemplateId = 32472
                    };
                    // message.setRecord(1, number);
                    // db.SmsRecords.Add(message.Record);
                    //await db.SaveChangesAsync();
                    await UserManager.SmsService.SendAsync(message);
                }
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            var result = new ReceiptVM()
            {
                Id = receipt.Id,
                PostUserId = receipt.PostUserId,
                ConfirmerID = null,
                CreateTime = receipt.CreateTime,
                ConfirmTime = receipt.ConfirmTime,
                State = receipt.State,
                Value = 0,
                PosterName = receiveuser.Name,
                CheckUserId = 0
            };
            return Ok(result);



        }





        // GET: api/Receipts/5
        [ResponseType(typeof(PageResult<ReceiptVM>))]
        public async Task<IHttpActionResult> GetReceipt(int postId, int state, string key, int pageSize, int page,string order,bool isAsc)
        {
            PostUser postUser = await db.PostUsers.FindAsync(postId);
            if (postUser == null)
            {
                return NotFound();
            }
            PostManager pm = new PostManager();
            var postids = pm.GetPostUserIdsByPostCampus(postUser.PostId).Select(p=>p.Id);
            PageResult<ReceiptVM> result = new PageResult<ReceiptVM>();
            List<ReceiptVM> data = new List<ReceiptVM>();
            var receipt = db.Receipts.Include(o => o.Compensations).Include(o => o.Orders).Where(o => postids.Contains(o.PostUser.PostId))
                .Where(o => state < 0 ? true : (int)o.State == state)
                .Where(o=>o.Compensations.Where(or => or.State ==true).Count()>0||o.Orders.Where(or=>or.State!=OrderState.已删除).Count()>0)       
                .Where(o => (key == "" || key == null) ? true : o.PostUser.User.Name.IndexOf(key) > -1
                        );
            var rec = isAsc ? LinqOrder.DataSort(receipt, order, "asc") : LinqOrder.DataSort(receipt, order, "desc");
            var count = rec.Select(o => o.Id).Count();
            if (count == 0)
            {
                result.Count = 0;
                result.Data = null;
                result.CurrentPage = 1;
                result.Order = order;
                result.IsAsc = isAsc;
                result.PageSize = pageSize;
                return Ok(result);
            }
            result.Count = count;
            var list= await rec.Skip((page - 1) * pageSize).Take(pageSize).Select(o => new ReceiptVM()
            {
               Id=o.Id,
                PostUserId=o.PostUserId,
                 ConfirmerID=o.ConfirmerID,
                  CreateTime=o.CreateTime,
                   ConfirmTime=o.ConfirmTime,
                    State=o.State,
                     Value=0,
                     PosterName=o.PostUser.User.Name,
                     CheckUserId=postId
            }).ToListAsync(); 
            foreach(var r in list)
            {
                var ords = db.Orders.Where(o => o.ReceiptID == r.Id).Where(o => o.State != OrderState.已删除);
                //if (ords == null)//如果订单为空  说明是空单据  删除
                //{
                //    list.Remove(r);
                //    continue;
                //}
                if (ords.Count()>0)
                {
                    r.Value = r.Value + ords.Sum(o => o.OrderDetails.Sum(d => d.ActualPay));
                }
                var coms = db.Compensations.Where(o => o.ReceiptID == r.Id);
                if (coms.Count() > 0)
                {
                    r.Value = r.Value + coms.Sum(o => o.Value);
                }
            }
            result.Data = list;             
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result);
        }

        // PUT: api/Receipts/5
        [ResponseType(typeof(Receipt))]
        public async Task<IHttpActionResult> PutReceipt(int id, Receipt receipt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != receipt.Id)
            {
                return BadRequest();
            }
            if (receipt.State == ReceiptState.待结账)
            {
                if (checkDebt(receipt))
                {
                    receipt.State = ReceiptState.有欠费;
                }else
                {
                    receipt.State = ReceiptState.已结清;
                }

            }
            if (receipt.State == ReceiptState.补费待结)
            {
                receipt.State = ReceiptState.已结清;
                checkCompensation(receipt);
            }
            receipt.ConfirmTime = DateTime.UtcNow;
            db.Entry(receipt).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReceiptExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            receipt.Orders = null;
            receipt.Compensations = null;
            return Ok(receipt);
        }

        // POST: api/Receipts
        [ResponseType(typeof(Receipt))]
        public async Task<IHttpActionResult> PostReceipt(Receipt receipt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            receipt.CreateTime = DateTime.UtcNow;
            db.Receipts.Add(receipt);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = receipt.Id }, receipt);
        }

        // DELETE: api/Receipts/5
        [ResponseType(typeof(Receipt))]
        public async Task<IHttpActionResult> DeleteReceipt(int id)
        {
            Receipt receipt = await db.Receipts.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }

            db.Receipts.Remove(receipt);
            await db.SaveChangesAsync();

            return Ok(receipt);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
                db.Dispose();
            }

            base.Dispose(disposing);
            
        }

        private bool ReceiptExists(int id)
        {
            return db.Receipts.Count(e => e.Id == id) > 0;
        }

        private List<int> getSonIds(int id)
        {
            PostManager pm = new PostManager();
            return pm.GetSons(id).Select(p => p.Id).ToList();
        }

        private bool checkDebt(Receipt rec)
        {
            var result = false;
            var ords = db.Orders.Where(o => o.ReceiptID == rec.Id);
            foreach(var order in ords)
            {
                if (order.IsDebt)
                {
                    result = true;
                    order.State = OrderState.有欠费;
                }else
                {
                    order.State = OrderState.已支付;
                }
                db.Entry(order).State = EntityState.Modified;
            }
            return result;
        }
        private void checkCompensation(Receipt rec)
        {
            var coms = db.Compensations.Where(o => o.ReceiptID == rec.Id);
            foreach(var com in coms)
            {
                var details = com.Order.OrderDetails;
                var all = details.Sum(d => d.ActualPay);
                var alldebt = details.Sum(d => d.Debt);
                foreach (var d in details)
                {
                    var val = Math.Round(com.Value * d.ActualPay / all);
                    d.ActualPay = d.ActualPay + val;
                    var debt= Math.Round(com.Value * d.Debt / alldebt);
                    d.Debt = d.Debt - debt;
                    db.Entry(d).State = EntityState.Modified;
                }
                if (com.Value >= alldebt)
                {
                    com.Order.IsDebt = false;
                    com.Order.State = OrderState.已支付;
                    db.Entry(com.Order).State = EntityState.Modified;
                }else
                {
                    com.Order.State = OrderState.有欠费;
                    db.Entry(com.Order).State = EntityState.Modified;
                }
            }

            foreach(var com in coms)
            {
                var ord = db.Orders.Where(o => o.ReceiptID == com.Order.ReceiptID).Where(o => o.IsDebt);
                if (ord.Count() <= 0)
                {
                    com.Order.Receipt.State = ReceiptState.已结清;
                    db.Entry(com.Order.Receipt).State = EntityState.Modified;
                }
            }        
        }
    }
}