using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FYstudentMgr.Models
{
    public enum OrderState
    {
        待支付,待结账,已支付, 有欠费,补费待结, 已删除
    }

    public enum PayChannel
    {
        支付宝, 微信,现金,银行卡,信用卡
    }
    public class Order
    {
        public int Id { get; set; }      
        public string OrderNO { get; set; }//订单编号
        public int StudentID { get; set; }//学生id
        public int PostUserId  { get; set; }//经办业务的岗位id
        
        public DateTime OrderDate { get; set; }//学下单时间
        public int? ReceiptID { get; set; }//所属单据id
        //public double ActualPay { get; set; }//实际支付金额
        public OrderState State { get; set; } //订单状态  
        public DateTime PayDate { get; set; }//支付时间 预留为实现在线支付
        public string TradeNO { get; set; } //支付单号  前期用来保存收据编号  后期在线支付用来保存支付的编号
        public PayChannel Channel { get; set; }//支付渠道
        public bool IsDebt { get; set; }//是否欠费pub
        public bool IsOtherDiscount { get; set; }
        //public double Debt { get; set; }//欠费金额
        public virtual Student Student { get; set; }
        public string Remark { get; set; }
        public virtual PostUser postUser { get; set; }
        public virtual Receipt Receipt { get; set; }
        

        public virtual ICollection<Compensation> Compensations { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}