using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class ClassTeacher
    {
        public int Id { get; set; }
        public int ClassId { get; set; }//班级id
        public int TeacherId { get; set; }//教师岗位id
        public string Course { get; set; }//教授课程
        public virtual PostUser Teacher { get; set; }
        public virtual Class Class { get; set; }
    }
}
