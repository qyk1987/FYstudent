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
using JsonPatch;

namespace FYstudentMgr.Controllers
{
    [Authorize]
   // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CouponsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Coupons
        public IQueryable<Coupon> GetCoupons()
        {
            return db.Coupons;
        }

        // GET: api/Coupons
        public IQueryable<Coupon> GetCoupons(int campusId)
        {
            return db.CampusCoupons.Where(C=>C.CampusID==campusId)
                    .Where(C=>C.Coupon.State)
                    .Select(C=>C.Coupon);
        }

        // GET: api/Coupons/5
        [ResponseType(typeof(Coupon))]
        public async Task<IHttpActionResult> GetCoupon(int id)
        {
            Coupon coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }

            return Ok(coupon);
        }


        // GET: api/Receipts/5
        [ResponseType(typeof(PageResult<CouponVM>))]
        public async Task<IHttpActionResult> GetReceipt( bool state, string key, int pageSize, int page, string order, bool isAsc)
        {
            PageResult<CouponVM> result = new PageResult<CouponVM>();
            List<CouponVM> data = new List<CouponVM>();
            var coupon = db.Coupons.Include(o => o.Products).Include(o => o.Campuses).Where(c=>c.State==state)
                .Where(o => (key == "" || key == null) ? true : o.CouponName.IndexOf(key) > -1
                        );
            var rec = isAsc ? LinqOrder.DataSort(coupon, order, "asc") : LinqOrder.DataSort(coupon, order, "desc");
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
            var list = await rec.Skip((page - 1) * pageSize).Take(pageSize).Select(o => new CouponVM()
            { 
                 CouponName=o.CouponName,
                Id = o.Id,
                OverDate = o.OverDate,
                Rule = o.Rule,
                StartDate = o.StartDate,
                State = o.State,
                Vlaue = o.Vlaue,
                 campusIds=o.Campuses.Select(c=>c.CampusID).ToList(),
                  productIds=o.Products.Select(c=>c.ProductId).ToList()
                
            }).ToListAsync();
            result.Data = list;
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result);
        }

        // PUT: api/Coupons/5
        [ResponseType(typeof(CouponVM))]
        public async Task<IHttpActionResult> PutCoupon(int id, Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != coupon.Id)
            {
                return BadRequest();
            }

            db.Entry(coupon).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            coupon = await db.Coupons.Include(cou => cou.Campuses).Include(c => c.Products).Where(c => c.Id == id).SingleOrDefaultAsync();
            var data= new CouponVM()
            {
                CouponName = coupon.CouponName,
                Id = coupon.Id,
                OverDate = coupon.OverDate,
                Rule = coupon.Rule,
                StartDate = coupon.StartDate,
                State = coupon.State,
                Vlaue = coupon.Vlaue,
                campusIds = coupon.Campuses.Select(c => c.CampusID).ToList(),
                productIds = coupon.Products.Select(c => c.ProductId).ToList()

            };
            return Ok(data);
        }

        [ResponseType(typeof(CouponVM))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<Coupon> newdata)
        {
            var coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(coupon);
            await db.SaveChangesAsync();
            coupon = await db.Coupons.Include(cou => cou.Campuses).Include(c => c.Products).Where(c => c.Id == id).SingleOrDefaultAsync();
            var data = new CouponVM()
            {
                CouponName = coupon.CouponName,
                Id = coupon.Id,
                OverDate = coupon.OverDate,
                Rule = coupon.Rule,
                StartDate = coupon.StartDate,
                State = coupon.State,
                Vlaue = coupon.Vlaue,
                campusIds = coupon.Campuses.Select(c => c.CampusID).ToList(),
                productIds = coupon.Products.Select(c => c.ProductId).ToList()

            };
            return Ok(data);
        }

        // PUT: api/Coupons/5
        [ResponseType(typeof(CouponVM))]
        public async Task<IHttpActionResult> PutCampus(int id,bool cam, List<int> pids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var campus = db.CampusCoupons.Where(c => c.CouponID == id).ToList();
            var oldids = campus.Select(c => c.CampusID);
            foreach (var camp in campus) //删除校区
            {
                if (!pids.Contains(camp.CampusID))
                {
                    db.CampusCoupons.Remove(camp);
                }
            }
            foreach (var cid in pids) //新增校区
            {
                if (!oldids.Contains(cid))
                {
                    CampusCoupon cc = new Models.CampusCoupon();
                    cc.CampusID = cid;
                    cc.CouponID = id;
                    db.CampusCoupons.Add(cc);
                }
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var coupon =await db.Coupons.Include(c => c.Products).Include(c => c.Campuses).SingleOrDefaultAsync(c => c.Id ==id);
            var data = new CouponVM()
            {
                CouponName = coupon.CouponName,
                Id = coupon.Id,
                OverDate = coupon.OverDate,
                Rule = coupon.Rule,
                StartDate = coupon.StartDate,
                State = coupon.State,
                Vlaue = coupon.Vlaue,
                campusIds = coupon.Campuses.Select(c => c.CampusID).ToList(),
                productIds = coupon.Products.Select(c => c.ProductId).ToList()

            };
            return Ok(data);
        }

        // PUT: api/Coupons/5
        [ResponseType(typeof(CouponVM))]
        public async Task<IHttpActionResult> PutProducts(int id, bool prd, List<int> pids)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = db.CouponProducts.Where(c => c.CouponId == id).ToList();
            var oldids = products.Select(c => c.ProductId);
            foreach (var prdt in products) //删除产品
            {
                if (!pids.Contains(prdt.ProductId))
                {
                    db.CouponProducts.Remove(prdt);
                }
            }
            foreach (var cid in pids) //新增产品
            {
                if (!oldids.Contains(cid))
                {
                   CouponProduct cc = new CouponProduct();
                    cc.ProductId = cid;
                    cc.CouponId = id;
                    db.CouponProducts.Add(cc);
                }
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CouponExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var coupon = await db.Coupons.Include(c => c.Products).Include(c => c.Campuses).SingleOrDefaultAsync(c => c.Id == id);
            var data = new CouponVM()
            {
                CouponName = coupon.CouponName,
                Id = coupon.Id,
                OverDate = coupon.OverDate,
                Rule = coupon.Rule,
                StartDate = coupon.StartDate,
                State = coupon.State,
                Vlaue = coupon.Vlaue,
                campusIds = coupon.Campuses.Select(c => c.CampusID).ToList(),
                productIds = coupon.Products.Select(c => c.ProductId).ToList()

            };
            return Ok(data);
        }

        // POST: api/Coupons
        [ResponseType(typeof(CouponVM))]
        public async Task<IHttpActionResult> PostCoupon(Coupon coupon)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            coupon.StartDate = DateTime.UtcNow;
            db.Coupons.Add(coupon);
            await db.SaveChangesAsync();
            var cou = await db.Coupons.Include(c=>c.Products).Include(c=>c.Campuses).SingleOrDefaultAsync(c=>c.Id==coupon.Id);
            var data = new CouponVM()
            {
                CouponName = coupon.CouponName,
                Id = coupon.Id,
                OverDate = coupon.OverDate,
                Rule = coupon.Rule,
                StartDate = coupon.StartDate,
                State = coupon.State,
                Vlaue = coupon.Vlaue,
                campusIds = coupon.Campuses.Select(c => c.CampusID).ToList(),
                productIds = coupon.Products.Select(c => c.ProductId).ToList()

            };
            return CreatedAtRoute("DefaultApi", new { id = data.Id }, data);
        }

        // DELETE: api/Coupons/5
        [ResponseType(typeof(Coupon))]
        public async Task<IHttpActionResult> DeleteCoupon(int id)
        {
            Coupon coupon = await db.Coupons.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            var campus = db.CampusCoupons.Where(c => c.CouponID == id);
            db.CampusCoupons.RemoveRange(campus);
            var products=db.CouponProducts.Where(c => c.CouponId == id);
            db.CouponProducts.RemoveRange(products);
            db.Coupons.Remove(coupon);
            
            await db.SaveChangesAsync();

            return Ok(coupon);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CouponExists(int id)
        {
            return db.Coupons.Count(e => e.Id == id) > 0;
        }
    }
}