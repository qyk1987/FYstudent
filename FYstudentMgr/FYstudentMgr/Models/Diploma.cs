using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Diploma
    {
        public int Id { get; set; }
        public string DiplomaName { get; set; }//证书名称
        public bool DiplomaState { get; set; }
        public DateTime CreateDate { get; set; }
        public virtual ICollection<StudentDiploma> UserDiploms { get; set; }
    }
}