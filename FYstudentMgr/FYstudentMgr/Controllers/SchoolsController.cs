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
    public class SchoolsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Schools
        public IQueryable<School> GetSchools()
        {
            return db.Schools;
        }



        // GET: api/Schools/5
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> GetSchool(int id)
        {
            School school = await db.Schools.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            return Ok(school);
        }



        //GET: api/Schools/5
        [ResponseType(typeof(PageResult<School>))]
        public async Task<IHttpActionResult> GetSchool(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<School> result = new PageResult<School>();
            IOrderedQueryable<School> schools= db.Schools;
            var count = schools.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(schools, order, "asc") : LinqOrder.DataSort(schools, order, "desc");
            //schools = isAsc ? schools.OrderBy(order):schools.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }



        // PUT: api/Schools/5
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> PutSchool(int id, School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != school.Id)
            {
                return BadRequest();
            }

            db.Entry(school).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(school);
        }

        [ResponseType(typeof(School))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<School> newdata)
        {
            var old = await db.Schools.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }


        // POST: api/Schools
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> PostSchool(School school)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            school.CreateDate = DateTime.UtcNow;
            db.Schools.Add(school);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = school.Id }, school);
        }

        // DELETE: api/Schools/5
        [ResponseType(typeof(School))]
        public async Task<IHttpActionResult> DeleteSchool(int id)
        {
            School school = await db.Schools.FindAsync(id);
            if (school == null)
            {
                return NotFound();
            }

            db.Schools.Remove(school);
            await db.SaveChangesAsync();

            return Ok(school);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SchoolExists(int id)
        {
            return db.Schools.Count(e => e.Id == id) > 0;
        }
    }
}