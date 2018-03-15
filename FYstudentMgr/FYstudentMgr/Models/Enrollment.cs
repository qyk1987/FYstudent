using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public enum EnrollmentState
    {
        试听, 缴费, 重修,休学
    }
    public class Enrollment
    {
        public int Id { get; set; }
        public int ClassID { get; set; }
        public int StudentID { get; set; }
       
        public DateTime EnrollDate { get; set; }
        public EnrollmentState EnrollmentState { get; set; }

        public virtual Class Class { get; set; }
        public virtual Student Student { get; set; }
    }
}