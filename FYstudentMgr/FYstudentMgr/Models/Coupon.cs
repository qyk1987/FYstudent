using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string CouponName { get; set; }//优惠名称
        public double Vlaue { get; set; }//优惠金额
        public string Rule { get; set; }//使用规则
        public DateTime StartDate { get; set; }//开始时间
        public DateTime OverDate { get; set; }//结束时间
        public bool State { get; set; }//优惠政策状态  是否可用

        public virtual ICollection<CampusCoupon> Campuses { get; set; }
        public virtual ICollection<CouponProduct> Products { get; set; }
    }
}