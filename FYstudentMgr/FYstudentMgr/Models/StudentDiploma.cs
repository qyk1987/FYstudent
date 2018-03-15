using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class StudentDiploma
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int DiplomaID { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual Diploma Diploma { get; set; }
        public virtual Student Student { get; set; }
    }
}