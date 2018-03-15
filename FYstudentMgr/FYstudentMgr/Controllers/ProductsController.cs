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
using FYstudentMgr.Manager;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    [RoutePrefix("api/Products")]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private PostManager postmanager = new PostManager();
        // GET: api/Products
        public IQueryable<SimpleProduct> GetProducts()
        {
            return db.Products.Select(p=>new SimpleProduct() {
                 Id=p.Id,
                  ProductName=p.ProductName,
                   SubjectId=p.SubjectId
            });
        }
        /// <summary>
        /// 根据subject查询产品
        /// </summary>
        /// <param name="id"></param>
        /// <param name="foo"></param>
        /// <returns></returns>
        public IQueryable<Product> GetProducts(int id,bool foo)
        {
            return db.Products.Where(p=>p.SubjectId==id);
        }
        /// <summary>
        /// 根据校区查询产品,携带couponIds，因为不同校区优惠券不一样
        /// </summary>
        /// <param name="id"></param>
        /// <param name="foo"></param>
        /// <returns></returns>
        public IQueryable<ProductWithSubjectAndIds> GetProducts(int campusId)
        {
            return db.Products.Where(p=>p.State)
                        .Select(p=>new ProductWithSubjectAndIds() {
                             Id=p.Id,
                             AccordIdList=p.AccordIdList,
                             CreateDate=p.CreateDate,
                             CategoryId=p.Subject.CategoryId,
                             SubjectName=p.Subject.Name,
                             SubjectId=p.SubjectId,
                             OverDate=p.OverDate,
                             Price=p.Price,
                             IsDiscountForOld=p.IsDiscountForOld,
                             DiscountValue=p.DiscountValue,
                             ProductName=p.ProductName,
                             Sort=p.Sort,
                             State=p.State,
                             couponIds=p.Coupons.Where(c=>c.Coupon.State&&c.Coupon.Campuses.Select(cam=>cam.CampusID).Contains(campusId)).Select(c=>c.CouponId).ToList()
                        });
        }
        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/Districts
        [ResponseType(typeof(List<TreeItem>))]
        public async Task<IHttpActionResult> GetMenu(int menu)
        {
            var list = db.Categorys.Select(d => new TreeItem()
            {

                Id = d.Id
            }).ToList();
            foreach (var cat in list)
            {
                cat.items = db.Subjects.Where(c =>  c.CategoryId == cat.Id).Select(c => new TreeItem()
                {

                    Id = c.Id
                }).ToList();
            }
            return Ok(list);
        }


        //GET: api/Districts/5
        [ResponseType(typeof(PageResult<ProductVM>))]
        public async Task<IHttpActionResult> GetProducts(int level, int id, int pageSize, int page, string order, bool isAsc)
        {
            PageResult<ProductVM> result = new PageResult<ProductVM>();
            var products = level == -1 ? db.Products :
                        level == 0 ? db.Products.Where(p => p.Subject.CategoryId==id) :
                        db.Products.Where(p => p.SubjectId == id);
            products = products.Include(p => p.Services).Include(p => p.Coupons);
            var count = products.Count();
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
            var ord = isAsc ? LinqOrder.DataSort(products, order, "asc") : LinqOrder.DataSort(products, order, "desc");
            var data = await ord.Skip((page - 1) * pageSize).Take(pageSize).Select(p=>new ProductVM() {
                 Id=p.Id,
                IsDiscountForOld = p.IsDiscountForOld,
                IsPackage = p.IsPackage,
                OverDate = p.OverDate,
                PackageIdList = p.PackageIdList,
                Price = p.Price,
                ProductName = p.ProductName,
                SaleCount = p.SaleCount,
                Sort = p.Sort,
                State = p.State,
                SubjectId = p.SubjectId,
                DiscountValue = p.DiscountValue,
                Desc = p.Desc,
                CoverImg = p.CoverImg,
                CreateDate = p.CreateDate,
                AccordIdList = p.AccordIdList,
                 couponIds=p.Coupons.Where(c => c.Coupon.State).Select(c=>c.CouponId).ToList(),
                  serviceIds=p.Services.Select(s=>s.ServiceId).ToList()

            }).ToListAsync();
            
            result.Data = data;
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }

        // PUT: api/Products/5
        [ResponseType(typeof(ProductVM))]
        public async Task<IHttpActionResult> PutProduct(int id, ProductVM product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }
            var prd = await db.Products.FindAsync(id);
            if (product.State == false&& prd.State==true)
            {
                prd.OverDate = DateTime.UtcNow;
            }
            prd.AccordIdList = product.AccordIdList;
            prd.CoverImg = product.CoverImg;
            prd.DiscountValue = product.DiscountValue;
            prd.Desc = product.Desc;
            prd.IsDiscountForOld = product.IsDiscountForOld;
            prd.IsPackage = product.IsPackage;
            prd.PackageIdList = product.PackageIdList;
            prd.Price = product.Price;
            prd.ProductName = product.ProductName;
            db.Entry(prd).State = EntityState.Modified;

            var newids = product.couponIds;
            var coupons = db.CouponProducts.Where(c => c.ProductId == product.Id).Where(c => c.Coupon.State).ToList();
            var oldids = coupons.Select(c=>c.CouponId);
            foreach (var coupon in coupons) //删除优惠券
            {
                if (!newids.Contains(coupon.CouponId))
                {
                    db.CouponProducts.Remove(coupon);
                }
            }
            foreach (var cid in product.couponIds) //新增优惠券
            {
                if (!oldids.Contains(cid))
                {
                    CouponProduct cp = new CouponProduct();
                    cp.CouponId = cid;
                    cp.ProductId = id;
                    db.CouponProducts.Add(cp);
                }
            }

            var newids2 = product.serviceIds;
            var services = db.ProductServices.Where(c => c.ProductId == product.Id).ToList();
            var oldids2 = services.Select(c=>c.ServiceId);
            foreach (var service in services) //删除服务
            {
                if (!newids2.Contains(service.ServiceId))
                {
                    db.ProductServices.Remove(service);
                }
            }
            foreach (var sid in product.serviceIds) //新增服务
            {
                if (!oldids2.Contains(sid))
                {
                    ProductService ps = new ProductService();
                    ps.ProductId = id;
                    ps.ServiceId = sid;
                    ps.Sort = 1;
                  
                    db.ProductServices.Add(ps);
                }
            }
           
            
            try
            {

                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
             
         
            return Ok(product);
        }

        // POST: api/Products
        [ResponseType(typeof(ProductVM))]
        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            product.CreateDate = DateTime.UtcNow;
            product.OverDate = DateTime.UtcNow;
            db.Products.Add(product);
            if (product.Coupons.Count > 0)
            {
                foreach (var coupon in product.Coupons)
                {
                    coupon.ProductId = product.Id;
                }
                db.CouponProducts.AddRange(product.Coupons);
            }
            if (product.Services.Count > 0)
            {
                foreach (var coupon in product.Services)
                {
                    coupon.ProductId = product.Id;
                }
                db.ProductServices.AddRange(product.Services);
            }

            await db.SaveChangesAsync();
            var p=  new ProductVM()
            {
                Id = product.Id,
                IsDiscountForOld = product.IsDiscountForOld,
                IsPackage = product.IsPackage,
                OverDate = product.OverDate,
                PackageIdList = product.PackageIdList,
                Price = product.Price,
                ProductName = product.ProductName,
                SaleCount = product.SaleCount,
                Sort = product.Sort,
                State = product.State,
                SubjectId = product.SubjectId,
                DiscountValue = product.DiscountValue,
                Desc = product.Desc,
                CoverImg = product.CoverImg,
                CreateDate = product.CreateDate,
                AccordIdList = product.AccordIdList,
                couponIds = product.Coupons.Select(c => c.CouponId).ToList(),
                serviceIds = product.Services.Select(s => s.ServiceId).ToList()

            };
            return CreatedAtRoute("DefaultApi", new { id = p.Id }, p);
        }
        /// <summary>
        /// 
        /// 根据岗位id和起止日期获取订单表中按产品分部情况
        /// </summary>
        /// <param name="postid"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [Route("GetProdByPost")]
        [ResponseType(typeof(ProductWithCount))]
        public async Task<IHttpActionResult> GetProdByPost(int postid, string startDate, string endDate)
        {
            DateTime sdate = Convert.ToDateTime(startDate.Replace("\"", "")).Date;
            DateTime edate = Convert.ToDateTime(endDate.Replace("\"", ""));
            var idlist = postmanager.GetSons(postid).Select(p => p.Id);
            var result = db.OrderDetails.Include(od => od.Order.postUser).Include(od => od.Product)
                .Where(od => idlist.Contains(od.Order.postUser.PostId))
                .Where(od => od.Order.OrderDate >= sdate)
                .Where(od => od.Order.OrderDate <= edate)
                .GroupBy(od => od.Product.Id)
                .Select(g => new ProductWithCount()
                {
                    Id = g.Key,
                    ProductName = g.FirstOrDefault().Product.ProductName,
                    Count = g.Count()
                });
            return Ok(result);

        }



        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var orders = db.OrderDetails.Where(or => or.ProductId == id).Count();
            if (orders > 0)
            {
                return NotFound();
            }
            var coupons = db.CouponProducts.Where(c => c.ProductId == id);
            db.CouponProducts.RemoveRange(coupons);
            var services = db.ProductServices.Where(s => s.ProductId == id);
            db.ProductServices.RemoveRange(services);
            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                postmanager.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}