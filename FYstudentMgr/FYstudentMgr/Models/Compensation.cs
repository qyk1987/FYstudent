using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public enum FeeType
    {
        补费,退费
    }
    public class Compensation
    {
        public int Id { get; set; }
        public int OrderID { get; set; }//退/补费的订单id
        public int? ReceiptID { get; set; }//所属的单据id
        public double Value { get; set; }//金额
        public FeeType FeeType { get; set; }//费用类型  是退费还是补费
        public bool State { get; set; } //补费结账状态  
        public PayChannel Channel { get; set; }//支付渠道
        public DateTime CreateTime { get; set; }//时间
        public virtual Order Order { get; set; }
        public virtual Receipt Receipt { get; set; }
    }
}