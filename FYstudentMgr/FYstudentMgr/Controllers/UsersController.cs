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
    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Users
        public IQueryable<UserView> GetApplicationUsers()
        {
            return db.Users.Select(u=>new UserView()
            {
                Id=u.Id,
                 Email=u.Email,
                  Name=u.Name,
                   UserName=u.UserName,
                    Img=u.Img,
                     IsUploaImg=u.IsUploaImg,
                      PhoneNumber=u.PhoneNumber
            });
        }

        //// GET: api/Users/5
        //[ResponseType(typeof(ApplicationUser))]
        //public async Task<IHttpActionResult> GetApplicationUser(int id)
        //{
        //    ApplicationUser applicationUser = await db.Users.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(applicationUser);
        //}

       // GET: api/Schools/5
        [ResponseType(typeof(PageResult<UserView>))]
        public async Task<IHttpActionResult> GetUsers(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<UserView> result = new PageResult<UserView>();
            var users = db.Users;
            var count = users.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(users, order, "asc") : LinqOrder.DataSort(users, order, "desc");
            //schools = isAsc ? schools.OrderBy(order):schools.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).Select(u => new UserView()
            {
                Email = u.Email,
                Id = u.Id,
                Img = u.Img,
                IsUploaImg = u.IsUploaImg,
                Name = u.Name,
                PhoneNumber = u.PhoneNumber,
                UserName = u.UserName
            }).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }

        [ResponseType(typeof(ApplicationUser))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(string id, JsonPatchDocument<ApplicationUser> newdata)
        {
            var old =  db.Users.Find(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }



        // PUT: api/Users/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutApplicationUser(int id, ApplicationUser applicationUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != applicationUser.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(applicationUser).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ApplicationUserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Users
        //[ResponseType(typeof(ApplicationUser))]
        //public async Task<IHttpActionResult> PostApplicationUser(ApplicationUser applicationUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.ApplicationUsers.Add(applicationUser);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = applicationUser.Id }, applicationUser);
        //}

        // DELETE: api/Users/5
        [ResponseType(typeof(ApplicationUser))]
        //public async Task<IHttpActionResult> DeleteApplicationUser(int id)
        //{
        //    ApplicationUser applicationUser = await db.ApplicationUsers.FindAsync(id);
        //    if (applicationUser == null)
        //    {
        //        return NotFound();
        //    }

        //    db.ApplicationUsers.Remove(applicationUser);
        //    await db.SaveChangesAsync();

        //    return Ok(applicationUser);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //private bool ApplicationUserExists(int id)
        //{
        //    return db.Users.Count(e => e.Id == id) > 0;
        //}
    }
}