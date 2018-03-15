using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class EnrollService
    {
        public int Id { get; set; }
        public int EnrollmentId { get; set; }
        public int ServiceId { get; set; }
        public int PostUserId { get; set; }
        public DateTime DueDate { get; set; }
        public virtual Enrollment Enrollment { get; set; }
        public virtual Service Service { get; set; }
        public virtual PostUser PostUser { get; set; }
        
    }
}