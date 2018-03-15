using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class PostUser
    {
        public int Id { get; set; }
        public int PostId { get; set; }//所属岗位id
        public string UserId { get; set; }//在岗员工id
        public bool IsOnDuty { get; set; }//是否在岗
        public DateTime PostDate { get; set; }//员工上岗时间
        public DateTime? OffDate { get; set; }//员工下岗时间
        public int? PosterID { get; set; }//上岗处理人
        public int? OfferID { get; set; }//下岗处理人
        public virtual Post Post { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}