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
    public class DiplomasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Diplomas
        public IQueryable<Diploma> GetDiplomas()
        {
            return db.Diplomas;
        }

        // GET: api/Diplomas/5
        [ResponseType(typeof(Diploma))]
        public async Task<IHttpActionResult> GetDiploma(int id)
        {
            Diploma diploma = await db.Diplomas.FindAsync(id);
            if (diploma == null)
            {
                return NotFound();
            }

            return Ok(diploma);
        }


        //GET: api/Schools/5
        [ResponseType(typeof(PageResult<Diploma>))]
        public async Task<IHttpActionResult> GetSchool(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<Diploma> result = new PageResult<Diploma>();
            IOrderedQueryable<Diploma> services = db.Diplomas;
            var count = services.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(services, order, "asc") : LinqOrder.DataSort(services, order, "desc");
            //schools = isAsc ? schools.OrderBy(order):schools.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }

        // PUT: api/Diplomas/5
        [ResponseType(typeof(Diploma))]
        public async Task<IHttpActionResult> PutDiploma(int id, Diploma diploma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != diploma.Id)
            {
                return BadRequest();
            }

            db.Entry(diploma).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiplomaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(diploma);
        }

        [ResponseType(typeof(Diploma))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<Diploma> newdata)
        {
            var old = await db.Diplomas.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }

        // POST: api/Diplomas
        [ResponseType(typeof(Diploma))]
        public async Task<IHttpActionResult> PostDiploma(Diploma diploma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            diploma.CreateDate = DateTime.UtcNow;
            db.Diplomas.Add(diploma);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = diploma.Id }, diploma);
        }

        // DELETE: api/Diplomas/5
        [ResponseType(typeof(Diploma))]
        public async Task<IHttpActionResult> DeleteDiploma(int id)
        {
            Diploma diploma = await db.Diplomas.FindAsync(id);
            if (diploma == null)
            {
                return NotFound();
            }

            db.Diplomas.Remove(diploma);
            await db.SaveChangesAsync();

            return Ok(diploma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiplomaExists(int id)
        {
            return db.Diplomas.Count(e => e.Id == id) > 0;
        }
    }
}