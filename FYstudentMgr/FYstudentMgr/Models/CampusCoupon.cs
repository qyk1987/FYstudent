using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class CampusCoupon
    {
        public int Id { get; set; }
        public int CouponID { get; set; }
        public int CampusID { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual Campus Campus { get; set; }
    }
}