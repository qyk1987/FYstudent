using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class CouponProduct
    {
        public int Id { get; set; }
        public int CouponId { get; set; }
        public int ProductId { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual Product Product { get; set; }
    }
}