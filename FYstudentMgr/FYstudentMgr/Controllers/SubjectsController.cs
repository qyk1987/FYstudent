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
using System.Web.Http.Cors;

namespace FYstudentMgr.Controllers
{
    [Authorize]
   // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class SubjectsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Subjects
        public IQueryable<Subject> GetSubjects()
        {
            return db.Subjects;
        }
        public IQueryable<Subject> GetSubjects(int id,bool foo)
        {
            return db.Subjects.Where(s=>s.CategoryId==id);
        }


        // GET: api/Subjects/5
        [ResponseType(typeof(Subject))]
        public async Task<IHttpActionResult> GetSubject(int id)
        {
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            return Ok(subject);
        }

        // PUT: api/Subjects/5
        [ResponseType(typeof(Subject))]
        public async Task<IHttpActionResult> PutSubject(int id, Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subject.Id)
            {
                return BadRequest();
            }

            db.Entry(subject).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(subject);
        }

        // POST: api/Subjects
        [ResponseType(typeof(Subject))]
        public async Task<IHttpActionResult> PostSubject(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Subjects.Add(subject);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = subject.Id }, subject);
        }

        // DELETE: api/Subjects/5
        [ResponseType(typeof(Subject))]
        public async Task<IHttpActionResult> DeleteSubject(int id)
        {
            Subject subject = await db.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }

            db.Subjects.Remove(subject);
            await db.SaveChangesAsync();

            return Ok(subject);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubjectExists(int id)
        {
            return db.Subjects.Count(e => e.Id == id) > 0;
        }
    }
}