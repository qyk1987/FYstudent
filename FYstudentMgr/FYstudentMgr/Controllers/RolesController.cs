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
using System.Web.Http.Cors;
using FYstudentMgr.ViewModels;
using System.Threading.Tasks;
using FYstudentMgr.Helper;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class RolesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Roles
        public IQueryable<ApplicationRole> GetApplicationRoles()
        {
            return db.Roles;
        }

        // GET: api/Roles/5
        [ResponseType(typeof(ApplicationRole))]
        public IHttpActionResult GetApplicationRole(string id)
        {
            ApplicationRole applicationRole = db.Roles.Find(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            return Ok(applicationRole);
        }

        [ResponseType(typeof(PageResult<ApplicationRole>))]
        public async Task<IHttpActionResult> GetRoles(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<ApplicationRole> result = new PageResult<ApplicationRole>();
            var users = db.Roles;
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
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).Select(u => new ApplicationRole()
            {
                Description=u.Description,
                 Id=u.Id,
                  Label=u.Label,
                   Name=u.Name
                    
            }).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }

        // PUT: api/Roles/5
        [ResponseType(typeof(ApplicationRole))]
        public IHttpActionResult PutApplicationRole(string id, ApplicationRole applicationRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != applicationRole.Id)
            {
                return BadRequest();
            }

            db.Entry(applicationRole).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationRoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(applicationRole);
        }

        // POST: api/Roles
        [ResponseType(typeof(ApplicationRole))]
        public IHttpActionResult PostApplicationRole(ApplicationRole applicationRole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Roles.Add(applicationRole);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ApplicationRoleExists(applicationRole.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = applicationRole.Id }, applicationRole);
        }

        // DELETE: api/Roles/5
        [ResponseType(typeof(ApplicationRole))]
        public IHttpActionResult DeleteApplicationRole(string id)
        {
            ApplicationRole applicationRole = db.Roles.Find(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            db.Roles.Remove(applicationRole);
            db.SaveChanges();

            return Ok(applicationRole);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ApplicationRoleExists(string id)
        {
            return db.Roles.Count(e => e.Id == id) > 0;
        }
    }
}