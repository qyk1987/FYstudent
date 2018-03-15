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
using Microsoft.AspNet.Identity;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Posts
        public IQueryable<SimplePost> GetPosts()
        {
            return db.Posts.Select(p=> new SimplePost() {
                 Id=p.Id,
                 PostName=p.PostName,
                 RoleId=p.RoleId
            });
        }


        // GET: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> GetPost(int id)
        {
            Post spot = await db.Posts.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            return Ok(spot);
        }
        //GET: api/Districts/5
        [ResponseType(typeof(PageResult<Post>))]
        public async Task<IHttpActionResult> GetPosts(int level,int id,int pageSize, int page, string order, bool isAsc)
        {
            PageResult<Post> result = new PageResult<Post>();
            var posts = level == -1 ? db.Posts :
                        level == 0 ? db.Posts.Where(p => p.DistrictId == id) :
                        level == 1 ? db.Posts.Where(p => p.CampusId == id) : db.Posts.Where(p => p.SpotId == id);


            //posts = posts
            //    .Select(s => new Post()
            //{
            //   CampusId=s.CampusId,
            //    CreaterId=s.CreaterId,
            //     Id=s.Id,
            //      CreateTime=s.CreateTime,
            //       DistrictId=s.DistrictId,
            //        SpotId=s.SpotId,
            //         RoleId=s.RoleId,
            //          State=s.State,
            //           SupperId=s.SupperId,
            //            UserId=s.UserId,
            //             PostName=s.PostName
                          
            //});

            var count = posts.Count();
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
            var schs = isAsc ? LinqOrder.DataSort(posts, order, "asc") : LinqOrder.DataSort(posts, order, "desc");
            //districts = isAsc ? districts.OrderBy(order):districts.OrderByDescending(order);
            var data= await schs.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            foreach(var p in data)
            {
                p.PostUsers = db.PostUsers.Where(po => po.PostId == p.Id).ToList();
                foreach( var pu in p.PostUsers)
                {
                    pu.Post = null;
                    pu.User = null;             
                }
            }
            result.Data = data;
            result.CurrentPage = page;
            result.Order = order;
            result.IsAsc = isAsc;
            result.PageSize = pageSize;
            return Ok(result); ;

        }


        // PUT: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> PutPost(int id, PostViewModel postvm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var post = postvm.Post;
            if (id != post.Id)
            {
                return BadRequest();
            }
            var old = await db.Posts.FindAsync(id);
            if (old.UserId != post.UserId)
            {
                var pu =await db.PostUsers.Where(p => p.UserId == old.UserId).Where(p => p.PostId == post.Id).Where(p => p.IsOnDuty).FirstOrDefaultAsync();
                if (pu != null)
                {
                    pu.OfferID = postvm.PostId;
                    pu.IsOnDuty = false;
                    pu.OffDate = DateTime.UtcNow;
                    db.Entry(pu).State= EntityState.Modified;
                }

                PostUser punew = new PostUser()
                {
                    IsOnDuty = true,
                    
                    PostDate = DateTime.UtcNow,
                    PosterID = postvm.PostId,
                    PostId = post.Id,
                    UserId = post.UserId
                };
                db.PostUsers.Add(punew);
                checkRole(post.UserId);
                checkRole(old.UserId);
            }
            old.CampusId = post.CampusId;
            old.CreaterId = post.CreaterId;
            old.CreateTime = post.CreateTime;
            old.DistrictId = post.DistrictId;
            old.PostName = post.PostName;
            old.RoleId = post.RoleId;
            old.SpotId = post.SpotId;
            old.State = post.State;
            old.SupperId = post.SupperId;
            old.UserId = post.UserId;

            db.Entry(old).State = EntityState.Modified;


            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(post);
        }

        // POST: api/Posts
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userid = User.Identity.GetUserId<string>();
           // var post = postvm.Post;
            post.CreateTime = DateTime.UtcNow;
            db.Posts.Add(post);
            PostUser pu = new PostUser()
            {
                 IsOnDuty=true,
                  
                   PostDate=DateTime.UtcNow,
                    PosterID=post.CreaterId,
                     PostId= post.Id,
                      UserId=post.UserId                              
            };
            checkRole(post.UserId);
            db.PostUsers.Add(pu);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
            {
                
                    throw;
                
            }
            post.PostUsers = null;
            return CreatedAtRoute("DefaultApi", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public async Task<IHttpActionResult> DeletePost(int id)
        {
            Post spot = await db.Posts.FindAsync(id);
            if (spot == null)
            {
                return NotFound();
            }

            db.Posts.Remove(spot);
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

        private void checkRole(string userid)
        {
            var newids = db.PostUsers.Where(p => p.UserId == userid).Where(p => p.IsOnDuty).Select(p=>p.Post.RoleId);
            var oldids = db.UserRoles.Where(p => p.UserId == userid).Select(r => r.RoleId);
            var delids = oldids.Where(id => !newids.Contains(id));
            var addids = newids.Where(id => !oldids.Contains(id));
            var dels = db.UserRoles.Where(p => p.UserId == userid).Where(p => delids.Contains(p.RoleId));
            db.UserRoles.RemoveRange(dels);
            foreach(var id in addids)
            {
                var role = new ApplicationUserRole()
                {
                    RoleId = id,
                    UserId = userid
                };
                db.UserRoles.Add(role);
            }
            

        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.Id == id) > 0;
        }
    }
}