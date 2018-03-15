using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYstudentMgr.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace FYstudentMgr.Manager
{
    public class PostManager
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 获取某岗位下属的所有岗位
        /// </summary>
        /// <param name="id">岗位id</param>
        ///// <returns>返回该岗位及其所有下属的岗位</returns>
        public IEnumerable<Post> GetSons(int id)
        {
            var post = db.Posts.Find(id);
            var role = db.Roles.Find(post.RoleId);
            if(role.Name== "Accounter")
            {
                if (post.CampusId == null)
                {
                    return db.Posts.Where(p => p.DistrictId == post.DistrictId);
                }else
                {
                    return db.Posts.Where(p => p.CampusId == post.CampusId);
                }
            }
            var query = from c in db.Posts
                        where c.Id == id
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
        }
        /// <summary>
        /// 获取该岗位所在校区的所有岗位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<Post> GetPostUserIdsByPostCampus(int id)
        {
            var post = db.Posts.Find(id);
            if (post.CampusId == null)
            {
                var list = new List<Post>();
                list.Add(post);
                return list;
            }else
            {
                return db.Posts.Where(p => p.CampusId == post.CampusId).ToList();
            }

        }

        public void Dispose()
        {
            db.Dispose();
        }


        #region util工具
        /// <summary>
        /// 获取某业务员下属的所有业务员
        /// </summary>
        /// <param name="p_id"></param>
        ///// <returns></returns>
        private IEnumerable<Post> GetSonID(int p_id)
        {
            var query = from c in db.Posts
                        where c.SupperId == p_id
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.Id)));
        }

        #endregion

    }
}