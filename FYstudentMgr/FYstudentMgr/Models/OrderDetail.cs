using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? CouponID { get; set; } //使用优惠id 
        public int CampusId { get; set; }//承担服务该学生的校区
        public bool IsDiscountForOld { get; set; }//是否老学员优惠
        public double Debt { get; set; }//欠费金额
        public double ActualPay { get; set; }//实付
        public double Discount { get; set; } //优惠（包含老学员优惠）  
        //public double? OldDiscountValue { get; set; }//老学员优惠金额
        //public double? CouponDiscountValue { get; set; }//优惠券优惠金额
        public bool State { get; set; }//订单状态 是否分配人员进行处理
        public virtual Order Order { get; set; }
        public virtual Coupon Coupon { get; set; }
        public virtual Product Product { get; set; }
        public virtual Campus Campus { get; set; }
    }
}