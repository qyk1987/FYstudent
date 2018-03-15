using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public class VisitList
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int VisitorID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime VisitDate { get; set; }
        public Boolean IsRead { get; set; }
        public int VisitTimes { get; set; }

        public VisitList() {
            this.VisitDate = DateTime.Now;
        }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("VisitorID")]
        public virtual ApplicationUser Visitor { get; set; }
       
    }
}