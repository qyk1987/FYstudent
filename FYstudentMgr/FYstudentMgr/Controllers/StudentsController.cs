using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FYstudentMgr.Models;
using FYstudentMgr.Manager;
using System.Web.Routing;

using FYstudentMgr.ViewModels;
using System.Web.Http.Cors;
using System.Web.OData;
namespace FYstudentMgr.Controllers
{
    [Authorize]
   // [EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        private StudentManager studentManager = new StudentManager();
        
        // GET: api/Students
        public IQueryable<Student> GetStudents()
        {
            return studentManager.GetStudents();
        }

        // GET: api/Students?instructorId=5
        //[ResponseType(typeof(PageResult<Student>))]
        //public async Task<IHttpActionResult> GetByWorker(string postIds, int pageSize, int page, int order, bool isAsc)
        //{
        //    int[] idsList = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(postIds);

        //    return Ok(studentManager.GetStudentsByPostIds(idsList,pageSize,page,order, isAsc));
        //}


        // GET: api/Students?key=5
        public IQueryable<Student> GetByKey(string key)
        {
            
            if (key == null||key=="")
            {
                return null;
            }
            return studentManager.GetStudentsByKey(key);
        }

        public IQueryable<SimpleStudentData> GetByLikeKey(string strLike)
        {

            if (strLike == null || strLike == "")
            {
                return null;
            }
            return studentManager.GetStudentsByLike(strLike);
        }

        // GET: api/Students?strQuery=""
        [ResponseType(typeof(ViewModels.PageResult<Student>))]
        public async Task<IHttpActionResult> GetByFilter(string strQuery,string key, int pageSize, int page, string order, bool isAsc)
        {
            FilterIds idsList=Newtonsoft.Json.JsonConvert.DeserializeObject<FilterIds>(strQuery);
            return Ok(await studentManager.GetStudentsByFilter(idsList,key, pageSize, page, order, isAsc));
        }


        // GET: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> GetStudent(int id)
        {
            Student student = await studentManager.GetStudent(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }


        // GET: api/Students?introId=5  获取筛选项结构
        [ResponseType(typeof(StudentFilter))]
        public async Task<IHttpActionResult> GetFilter(string postIds, bool foo)
        {
            int[] idsList = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(postIds);
            return Ok(await studentManager.GetFilterByPostIds(idsList));
        }


        // PUT: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PutStudent(int id, Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            try
            {
                await studentManager.Update(student);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!studentManager.StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(student); 
        }

        // pATCH: api/Students/5
        [ResponseType(typeof(Student))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, Delta<Student>  student)
        {

            var stu = await studentManager.modify(id, student);
            if (stu == null)
            {
                return NotFound();
            }
            return Ok(stu);
        }


        // POST: api/Students
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> PostStudent(Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            student.SignDate = DateTime.UtcNow;
            student = await studentManager.Add(student);
            if (student == null)
            {

                return BadRequest("已存在该学生！");
            }
            return CreatedAtRoute("DefaultApi", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(Student))]
        public async Task<IHttpActionResult> DeleteStudent(int id)
        {
            Student student = await studentManager.Delete(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                studentManager.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}