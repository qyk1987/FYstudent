using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string RoleId { get; set; }//岗位对应的角色id
        public string PostName { get; set; }//岗位名称
        public string UserId { get; set; }//在岗的人员Id
        public int? CreaterId { get; set; }//创建岗位的岗位id
        public int? SupperId { get; set; }//上司的岗位id
        public bool State { get; set; }//岗位状态
        public DateTime CreateTime { get; set; }//岗位创建时间
        public int? DistrictId { get; set; }
        public int? CampusId { get; set; }
        public int? SpotId { get; set; }
        //[ForeignKey("SupperId")]
        //public virtual Post Supper { get; set; }
        public virtual PostUser Creater { get; set; }//创建岗位的岗位id
        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual District District { get; set; }
        public virtual Campus Campus { get; set; }
        public virtual Spot Spot { get; set; }
        [ForeignKey("PostId")]
        public virtual ICollection<PostUser> PostUsers { get; set; }
    }
}