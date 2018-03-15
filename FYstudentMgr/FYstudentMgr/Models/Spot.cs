using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Spot
    {
        public int Id { get; set; }
        public string SpotName { get; set; }//服务点名称
        public string SpotAddress { get; set; }//服务点地址
        public bool SpotState { get; set; }//服务点状态
        public int CampusID { get; set; }//所属校区ID
        public DateTime CreateDate { get; set; }
        public virtual Campus Campus { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        

    }
}