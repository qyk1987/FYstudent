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
using System.Web.Http.Cors;
using JsonPatch;

namespace FYstudentMgr.Controllers
{
    [Authorize]
   // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CompensationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Compensations
        public IQueryable<Compensation> GetCompensations()
        {
            return db.Compensations;
        }

        // GET: api/Compensations/5
        [ResponseType(typeof(Compensation))]
        public async Task<IHttpActionResult> GetCompensation(int id)
        {
            Compensation compensation = await db.Compensations.FindAsync(id);
            if (compensation == null)
            {
                return NotFound();
            }

            return Ok(compensation);
        }

        // PUT: api/Compensations/5
        [ResponseType(typeof(Compensation))]
        public async Task<IHttpActionResult> PutCompensation(int id, Compensation compensation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != compensation.Id)
            {
                return BadRequest();
            }

            db.Entry(compensation).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompensationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           // var order = getOrder(compensation.OrderID);
            return Ok(removeNavAttr(compensation));
        }


        [ResponseType(typeof(Compensation))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<Compensation> newdata)
        {
            var old = await db.Compensations.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }

        /// <summary>
        /// 负责业务员新增补费
        /// </summary>
        /// <param name="compensation"></param>
        /// <returns></returns>
        // POST: api/Compensations
        [ResponseType(typeof(Compensation))]
        public async Task<IHttpActionResult> PostCompensation(Compensation compensation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var ord = await db.Orders.FindAsync(compensation.OrderID);
                if (ord == null)
                {
                    return BadRequest(ModelState);
                }
                var receipt = db.Receipts.Where(r => r.State == ReceiptState.补费待结).Where(o => o.PostUserId == ord.PostUserId).SingleOrDefault();
                if (receipt == null)
                {
                    receipt = new Receipt();
                    receipt.CreateTime = DateTime.UtcNow;
                    receipt.State = ReceiptState.补费待结;
                    receipt.PostUserId = ord.PostUserId;
                    db.Receipts.Add(receipt);
                }
                compensation.ReceiptID = receipt.Id;
                compensation.CreateTime = DateTime.UtcNow;
                ord.State = OrderState.补费待结;
                db.Entry(ord).State = EntityState.Modified;
                db.Compensations.Add(compensation);
                await db.SaveChangesAsync();
              //  var order = getOrder(compensation.OrderID);
                
            }
            catch (DbUpdateConcurrencyException)
            {
               
                    throw;
                
            }
            return Ok(removeNavAttr(compensation));
        }
        /// <summary>
        /// 业务员删除补费
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Compensations/5
        [ResponseType(typeof(Compensation))]
        public async Task<IHttpActionResult> DeleteCompensation(int id)
        {
            Compensation compensation = await db.Compensations.FindAsync(id);
            if (compensation == null)
            {
                return NotFound();
            }

            db.Compensations.Remove(compensation);
            var ord = await db.Orders.FindAsync(compensation.OrderID);
            ord.State = OrderState.有欠费;
            db.Entry(ord).State = EntityState.Modified;
            CheckOrderState(ord.PostUserId);
            await db.SaveChangesAsync();
            //var order = getOrder(compensation.OrderID);
            return Ok(removeNavAttr(compensation));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CompensationExists(int id)
        {
            return db.Compensations.Count(e => e.Id == id) > 0;
        }



        private Compensation removeNavAttr(Compensation comp)
        {
            return new Compensation
            {
                Channel = comp.Channel,
                CreateTime = comp.CreateTime,
                FeeType = comp.FeeType,
                Id = comp.Id,
                OrderID = comp.OrderID,
                ReceiptID = comp.ReceiptID,
                State = comp.State,
                Value = comp.Value
                
            };
        }

        private void CheckOrderState(int postId)
        {
            var orders = db.Orders.Where(o => o.State == OrderState.补费待结).Where(o => o.PostUserId == postId);
            if (orders == null)
            {
                var receipts = db.Receipts.Where(r => r.State == ReceiptState.补费待结).Where(r => r.PostUserId == postId);
                db.Receipts.RemoveRange(receipts);
            }
        }

        //private OrderAllInfoWithProductIds getOrder(int id)
        //{
        //    var ord = db.Orders.Include(o=>o.Compensations).Include(o => o.OrderDetails).Include(o => o.Student).Where(o=>o.Id==id).ToList();
        //    if (ord == null)
        //    {
        //        return null;
        //    }
        //    var data = ord.Select(order => new OrderAllInfoWithProductIds()
        //    {
        //        Id = order.Id,
        //        StudentID = order.StudentID,
        //        TradeNO = order.TradeNO,
        //        OrderNO = order.OrderNO,
        //        PostUserId = order.PostUserId,
        //        OrderDate = order.OrderDate,
        //        ReceiptID = order.ReceiptID,
        //        ActualPay = order.OrderDetails.Sum(d => d.ActualPay),
        //        State = order.State,
        //        PayDate = order.PayDate,
        //        Channel = order.Channel,
        //        IsDebt = order.IsDebt,
        //        IsOtherDiscount = order.IsOtherDiscount,
        //        Debt = order.OrderDetails.Sum(d => d.Debt),
        //        Remark = order.Remark,
        //        Student = order.Student,
        //        ProductIds = order.OrderDetails.Select(od => od.ProductId).ToList(),
        //        Compensations = order.Compensations.ToList(),
        //        HasCompensation = order.Compensations.Count() > 0
        //    }).ToList();
        //    return data[0];
        //}
    }
}