using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }//大区名称
        public string DistrictAddress { get; set; }//大区地址
        public bool DistrictState { get; set; }//大区状态
        public DateTime CreateDate { get; set; }
        public virtual ICollection<Campus> Campuses { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}