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
    public class DistrictsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Districts
        public IQueryable<District> GetDistricts()
        {
            return db.Districts;
        }


        // GET: api/Districts
        [ResponseType(typeof(TreeNode))]
        public async Task<IHttpActionResult> GetTree(int chart)
        {
            TreeNode tree = new TreeNode();
            tree.name = "飞扬总部";
            tree.value = db.Districts.Where(d => d.DistrictState).Count();
            tree.children = db.Districts.Where(d => d.DistrictState).Select(d => new TreeNode()
            {
                name = d.DistrictName, value = d.Posts.Where(p => p.State).Count(),
                id=d.Id
            }).ToList();
            foreach(var child in tree.children)
            {
                child.children = db.Campuses.Where(c => c.CampusState).Where(c => c.DistrictID == child.id)
                    .Select(c => new TreeNode()
                    {
                         name=c.CampusName, id=c.Id,value=c.Posts.Where(p => p.State).Count()
                    }).ToList();
                foreach(var cam in child.children)
                {
                    cam.children = db.Spots.Where(s => s.SpotState).Where(s => s.CampusID == cam.id)
                        .Select(s => new TreeNode()
                        {
                            id=s.Id,
                            name = s.SpotName,
                            value = s.Posts.Where(p => p.State).Count()
                        }).ToList();
                }
            }
            var post = db.Posts.Where(p => p.DistrictId == null && p.State);
            if (post.Count() > 0)
            {
                tree.children = tree.children.Union(post.Select(p => new TreeNode()
                {
                    id = 0,
                    name = p.PostName,
                    value = p.PostUsers.Count(),
                    
                })).ToList();
            }
            
            foreach(var child in tree.children)
            {
                if (child.id != 0)
                {
                    var post1 = db.Posts.Where(p => p.DistrictId == child.id && p.CampusId == null && p.State);
                    if (post1.Count() > 0)
                    {
                        child.children = child.children.Union(post1.Select(p => new TreeNode()
                        {
                            id = 0,
                            name = p.PostName,
                            value = p.PostUsers.Count(),
                           
                        }).ToList()).ToList();
                    }
                    foreach (var cam in child.children)
                    {
                        if (cam.id != 0)
                        {
                            var post2 = db.Posts.Where(p => p.DistrictId == child.id && p.CampusId == cam.id && p.SpotId == null && p.State);
                            if (post2.Count() > 0)
                            {
                                cam.children = cam.children.Union(post2.Select(p => new TreeNode()
                                {
                                    id = 0,
                                    name = p.PostName,
                                    value = p.PostUsers.Count(),

                                }).ToList()).ToList();
                            }
                            foreach (var spot in cam.children)
                            {
                                if (spot.id != 0)
                                {
                                    var post3 = db.Posts.Where(p => p.DistrictId == child.id && p.CampusId == cam.id && p.SpotId == spot.id && p.State);
                                    if (post3.Count() > 0)
                                    {
                                        spot.children = post3.Select(p => new TreeNode()
                                        {
                                            id = 0,
                                            name = p.PostName,
                                            value = p.PostUsers.Count(),

                                        }).ToList();
                                    }


                                }

                            }
                        }
                       

                    }

                }
               
            }
            return Ok(tree);
        }


        // GET: api/Districts
        [ResponseType(typeof(List<TreeItem>))]
        public async Task<IHttpActionResult> GetMenu(int menu)
        {
            var list = db.Districts.Where(d => d.DistrictState).Select(d => new TreeItem()
            {
               
                       Id=d.Id
            }).ToList();
            foreach(var dis in list)
            {
                dis.items = db.Campuses.Where(c => c.CampusState && c.DistrictID == dis.Id).Select(c => new TreeItem()
                {
               
                    Id = c.Id
                }).ToList();
                foreach(var cam in dis.items)
                {
                    cam.items = db.Spots.Where(s => s.SpotState && s.CampusID == cam.Id).Select(s => new TreeItem()
                    {
                       Id=s.Id
                        
                    }).ToList();
                }
            }
            return Ok(list);
        }

        // GET: api/Districts/5
        [ResponseType(typeof(District))]
        public async Task<IHttpActionResult> GetDistrict(int id)
        {
            District district = await db.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            return Ok(district);
        }



        //GET: api/Districts/5
        [ResponseType(typeof(PageResult<District>))]
        public async Task<IHttpActionResult> GetDistrict(int pageSize, int page, string order, bool isAsc)
        {
            PageResult<District> result = new PageResult<District>();
            IOrderedQueryable<District> districts = db.Districts;
            var count = districts.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(districts, order, "asc") : LinqOrder.DataSort(districts, order, "desc");
            //districts = isAsc ? districts.OrderBy(order):districts.OrderByDescending(order);
            result.Data = await schs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }



        // PUT: api/Districts/5
        [ResponseType(typeof(District))]
        public async Task<IHttpActionResult> PutDistrict(int id, District district)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != district.Id)
            {
                return BadRequest();
            }

            db.Entry(district).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DistrictExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(district);
        }

        [ResponseType(typeof(District))]
        [AcceptVerbs("PATCH")]
        public async Task<IHttpActionResult> PatchStudent(int id, JsonPatchDocument<District> newdata)
        {
            var old = await db.Districts.FindAsync(id);
            if (old == null)
            {
                return NotFound();
            }
            newdata.ApplyUpdatesTo(old);
            await db.SaveChangesAsync();
            return Ok(old);
        }


        // POST: api/Districts
        [ResponseType(typeof(District))]
        public async Task<IHttpActionResult> PostDistrict(District district)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            district.CreateDate = DateTime.UtcNow;
            db.Districts.Add(district);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = district.Id }, district);
        }

        // DELETE: api/Districts/5
        [ResponseType(typeof(District))]
        public async Task<IHttpActionResult> DeleteDistrict(int id)
        {
            District district = await db.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }

            db.Districts.Remove(district);
            await db.SaveChangesAsync();

            return Ok(district);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DistrictExists(int id)
        {
            return db.Districts.Count(e => e.Id == id) > 0;
        }
    }
}