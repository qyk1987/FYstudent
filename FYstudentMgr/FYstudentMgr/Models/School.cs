using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class School
    {
        public int Id { get; set; }
        public string SchoolName { get; set; }//学校名称       
        public DateTime CreateDate { get; set; }
    }
}