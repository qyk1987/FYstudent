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
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class StudentDiplomasController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/StudentDiplomas
        public IQueryable<StudentDiploma> GetStudentDiplomas()
        {
            return db.StudentDiplomas;
        }


        // GET: api/StudentDiplomas?studentId=5
        public IQueryable<StudentDiploma> GetStudentDiplomasByStudent(int studentId)
        {
            return db.StudentDiplomas.Where(u=>u.StudentID==studentId);
        }
        // GET: api/StudentDiplomas/5
        [ResponseType(typeof(StudentDiploma))]
        public async Task<IHttpActionResult> GetStudentDiploma(int id)
        {
            StudentDiploma userDiploma = await db.StudentDiplomas.FindAsync(id);
            if (userDiploma == null)
            {
                return NotFound();
            }

            return Ok(userDiploma);
        }

        // PUT: api/StudentDiplomas/5
        [ResponseType(typeof(StudentDiploma))]
        public async Task<IHttpActionResult> PutStudentDiploma(int id, StudentDiploma userDiploma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userDiploma.Id)
            {
                return BadRequest();
            }

            db.Entry(userDiploma).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentDiplomaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(userDiploma);
        }

        // POST: api/StudentDiplomas
        [ResponseType(typeof(StudentDiploma))]
        public async Task<IHttpActionResult> PostStudentDiploma(StudentDiploma userDiploma)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = db.StudentDiplomas.Where(s => s.DiplomaID == userDiploma.DiplomaID).Where(s => s.StudentID == userDiploma.StudentID).ToList();
            if (result.Count>0)
            {
                return BadRequest("已存在");
            }
            userDiploma.CreateTime = DateTime.UtcNow;
            db.StudentDiplomas.Add(userDiploma);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = userDiploma.Id }, userDiploma);
        }

        // DELETE: api/StudentDiplomas/5
        [ResponseType(typeof(StudentDiploma))]
        public async Task<IHttpActionResult> DeleteStudentDiploma(int id)
        {
            StudentDiploma userDiploma = await db.StudentDiplomas.FindAsync(id);
            if (userDiploma == null)
            {
                return NotFound();
            }

            db.StudentDiplomas.Remove(userDiploma);
            await db.SaveChangesAsync();

            return Ok(userDiploma);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentDiplomaExists(int id)
        {
            return db.StudentDiplomas.Count(e => e.Id == id) > 0;
        }
    }
}