using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FYstudentMgr.Models;
using FYstudentMgr.ViewModels;
using FYstudentMgr.Helper;
using System.Threading.Tasks;
using JsonPatch;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    [RoutePrefix("api/Classes")]
    public class ClassesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Classes
        public IQueryable<Class> GetClasses()
        {
            return db.Classes;
        }

        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult GetClass(int id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        /// <summary>
        /// 根据校区id返回教务所在校区所有班级产品菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Classes/5
        [ResponseType(typeof(List<MenuVM>))]
        [Route("GetMenu")]
        public IHttpActionResult GetMenu(int campusid)
        {
            var classes = db.Classes
                .Include(c=>c.Product.Subject.Category)
                .Where(c => c.CampusId == campusid);
            var cates = classes.GroupBy(c => c.Product.Subject.CategoryId)
                .Select(g => new MenuVM()
                {
                    name = g.FirstOrDefault().Product.Subject.Category.CategoryName,
                    lists = g.GroupBy(c => c.ProductID)
                            .Select(gc => new MenuItem()
                            {
                                id = gc.Key,
                                count = gc.Count(),
                                //isopen = false,
                                name = gc.FirstOrDefault().Product.ProductName,
                                //selected = false
                            }).ToList()

                }).ToList();
                return Ok(cates);

        }

        [ResponseType(typeof(List<ChargerVM>))]
        [Route("GetChargers")]
        public IHttpActionResult GetChargers(int campusid)
        {
            var result = db.Posts.Where(p => p.CampusId == campusid)
                .Where(p => p.State)
                .Where(p => p.Role.Name == "ClassCharger")
                .Select(p => new ChargerVM()
                {
                    Name = p.User.Name,
                    Id = p.Id
                });
            return Ok(result);

        }

        [Route("getByPage")]
        [ResponseType(typeof(PageResult<ClassVM>))]
        public async Task<IHttpActionResult> getByPage(int campusid,int productid,int pageSize, int page, string order, bool isAsc)
        {
            PageResult<ClassVM> result = new PageResult<ClassVM>();
            var classes = db.Classes.Include(c=>c.Charger.User).Include(c=> c.Enrollments).Include(c => c.Product).Where(c=>c.CampusId==campusid);
            classes = productid ==0 ? classes : classes.Where(c => c.ProductID == productid);
            var count = classes.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(classes, order, "asc") : LinqOrder.DataSort(classes, order, "desc");
            //districts = isAsc ? districts.OrderBy(order):districts.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize)
                .Select(c=>new ClassVM()
                {
                     Id=c.Id,
                      Arrange=c.Arrange,
                       ChargerID=c.ChargerID,
                        chargerName=c.Charger.User.Name,
                         ClassName=c.ClassName,
                          ClassState=c.ClassState,
                           OverDate=c.OverDate,
                           ProductID=c.ProductID,
                           ProductName=c.Product.ProductName,
                         
                            studentCount=c.StudentCount
                })
                .ToListAsync();

            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }

        // PUT: api/Classes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutClass(int id, Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != @class.Id)
            {
                return BadRequest();
            }

            db.Entry(@class).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Classes
        [ResponseType(typeof(ClassVM))]
        public IHttpActionResult PostClass(Class @class)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            @class.OverDate = @class.OverDate.AddHours(8);
            db.Classes.Add(@class);
            db.SaveChanges();
            var cla = db.Classes.Include(c => c.Product).Include(c=>c.Charger.User).Where(c => c.Id == @class.Id).FirstOrDefault();
            var result = new ClassVM()
            {
                Arrange = cla.Arrange,
                ChargerID = cla.ChargerID,
                chargerName = cla.Charger.User.Name,
                ClassName = cla.ClassName,
                ClassState = cla.ClassState,
                Id = cla.Id,
                OverDate = cla.OverDate,
                ProductID = cla.ProductID,
                ProductName = cla.Product.ProductName,
                studentCount = cla.StudentCount
            };
            return CreatedAtRoute("DefaultApi", new { id = @class.Id },result);
        }


        [ResponseType(typeof(ClassVM))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> Patch(int id, JsonPatchDocument<Class> newdata)
        {
            var clas = await db.Classes.FindAsync(id);
            if (clas == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(clas);
            await db.SaveChangesAsync();
            var cla = db.Classes.Include(c => c.Product).Include(c => c.Charger.User).Where(c => c.Id == id).FirstOrDefault();
            var result = new ClassVM()
            {
                Arrange = cla.Arrange,
                ChargerID = cla.ChargerID,
                chargerName = cla.Charger.User.Name,
                ClassName = cla.ClassName,
                ClassState = cla.ClassState,
                Id = cla.Id,
                OverDate = cla.OverDate,
                ProductID = cla.ProductID,
                ProductName = cla.Product.ProductName,
                studentCount = cla.StudentCount
            };
            return Ok(result);
        }

        // DELETE: api/Classes/5
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(int id)
        {
            Class @class = db.Classes.Find(id);
            if (@class == null)
            {
                return NotFound();
            }

            db.Classes.Remove(@class);
            db.SaveChanges();

            return Ok(@class);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ClassExists(int id)
        {
            return db.Classes.Count(e => e.Id == id) > 0;
        }
    }
}