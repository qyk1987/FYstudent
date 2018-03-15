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
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class CampusController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Campus
        public IQueryable<Campus> GetCampuses()
        {
            return db.Campuses;
        }

        public IQueryable<Campus> GetCampusesByDistrict(int id,bool foo)
        {
            return db.Campuses.Where(c=>c.DistrictID==id);
        }

        // GET: api/Campus/5
        [ResponseType(typeof(Campus))]
        public async Task<IHttpActionResult> GetCampus(int id)
        {
            Campus campus = await db.Campuses.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }

            return Ok(campus);
        }

        //GET: api/Districts/5
        [ResponseType(typeof(PageResult<Campus>))]
        public async Task<IHttpActionResult> GetCampus(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<Campus> result = new PageResult<Campus>();
            var campus = db.Campuses;
            var count = campus.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(campus, order, "asc") : LinqOrder.DataSort(campus, order, "desc");
            //districts = isAsc ? districts.OrderBy(order):districts.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }


        // PUT: api/Campus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCampus(int id, Campus campus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != campus.Id)
            {
                return BadRequest();
            }

            db.Entry(campus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampusExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(campus);
        }

        [ResponseType(typeof(Campus))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<Campus> newdata)
        {
            var old =await db.Campuses.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }

        // POST: api/Campus
        [ResponseType(typeof(Campus))]
        public async Task<IHttpActionResult> PostCampus(Campus campus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Campuses.Add(campus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = campus.Id }, campus);
        }

        // DELETE: api/Campus/5
        [ResponseType(typeof(Campus))]
        public async Task<IHttpActionResult> DeleteCampus(int id)
        {
            Campus campus = await db.Campuses.FindAsync(id);
            if (campus == null)
            {
                return NotFound();
            }

            db.Campuses.Remove(campus);
            await db.SaveChangesAsync();

            return Ok(campus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CampusExists(int id)
        {
            return db.Campuses.Count(e => e.Id == id) > 0;
        }
    }
}