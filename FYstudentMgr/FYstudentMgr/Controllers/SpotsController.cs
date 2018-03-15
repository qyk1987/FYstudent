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
    public class SpotsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Spots
        public IQueryable<Spot> GetSpots()
        {
            return db.Spots;
        }


        public IQueryable<Spot> GetSpots(int id,bool foo)
        {
            return db.Spots.Where(s=>s.CampusID==id);
        }
        // GET: api/Spots/5
        [ResponseType(typeof(Spot))]
        public async Task<IHttpActionResult> GetSpot(int id)
        {
            Spot spot = await db.Spots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            return Ok(spot);
        }
        //GET: api/Districts/5
        [ResponseType(typeof(PageResult<SpotView>))]
        public async Task<IHttpActionResult> GetCampus(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<SpotView> result = new PageResult<SpotView>();
            var campus = db.Spots.Select(s=>new SpotView() {
                    CampusID=s.CampusID, CreateDate=s.CreateDate, DistrictID=s.Campus.DistrictID,
                     Id=s.Id, SpotAddress=s.SpotAddress, SpotName=s.SpotName, SpotState=s.SpotState
            });
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


        // PUT: api/Spots/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpot(int id, Spot spot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != spot.Id)
            {
                return BadRequest();
            }

            db.Entry(spot).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpotExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(spot);
        }


        [ResponseType(typeof(Spot))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<Spot> newdata)
        {
            var old = await db.Spots.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }

        // POST: api/Spots
        [ResponseType(typeof(Spot))]
        public async Task<IHttpActionResult> PostSpot(Spot spot)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            spot.CreateDate = DateTime.UtcNow;
            db.Spots.Add(spot);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = spot.Id }, spot);
        }

        // DELETE: api/Spots/5
        [ResponseType(typeof(Spot))]
        public async Task<IHttpActionResult> DeleteSpot(int id)
        {
            Spot spot = await db.Spots.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            db.Spots.Remove(spot);
            await db.SaveChangesAsync();

            return Ok(spot);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpotExists(int id)
        {
            return db.Spots.Count(e => e.Id == id) > 0;
        }
    }
}