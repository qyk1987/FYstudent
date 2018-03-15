using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public enum    ReceiptState
    {
        未申请, 待结账, 已结清,有欠费,补费待结
    }
    public class Receipt
    {
        public int Id { get; set; }
        public int PostUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public ReceiptState State { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public int? ConfirmerID { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Compensation> Compensations { get; set; }
        public virtual PostUser Confirmer { get; set; }
        public virtual PostUser PostUser { get; set; }
    }
}