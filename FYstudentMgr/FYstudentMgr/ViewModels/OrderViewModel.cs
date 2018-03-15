using FYstudentMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class OrderViewModel
    {

    }
    public class OrderWithProductIds
    {
        public int Id { get; set; }
        public int StudentID { get; set; }//学生id
        public DateTime OrderDate { get; set; }
        public List<int> ProductIds { get; set; }
    }

    public class OrderAllInfoWithProductIds
    {
        public int Id { get; set; }
        public string OrderNO { get; set; }//订单编号
        public int StudentID { get; set; }//学生id
        public int PostUserId { get; set; }//经办业务的岗位id
        public DateTime OrderDate { get; set; }//学下单时间
        public int? ReceiptID { get; set; }//所属单据id
        public double ActualPay { get; set; }//实际支付金额
        public OrderState State { get; set; } //订单状态  
        public DateTime PayDate { get; set; }//支付时间 预留为实现在线支付
        public string TradeNO { get; set; } //支付单号  前期用来保存收据编号  后期在线支付用来保存支付的编号
        public PayChannel Channel { get; set; }//支付渠道
        public bool IsDebt { get; set; }//是否欠费pub
        public bool IsOtherDiscount { get; set; }
        public double Debt { get; set; }//欠费金额
        public string Remark { get; set; }
        public Student Student { get; set; }
        public List<int> ProductIds { get; set; }
        public bool HasCompensation { get; set; }
        public List<Compensation> Compensations { get; set; }
    }

    public class OrderResult
    {
        public List<OrderAllInfoWithProductIds> orders { get; set; }
        public List<StatiticDataItem> statistic { get; set; }
    }

    public class StatiticDataItem
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
        public double Pay { get; set; }
        public double Debt { get; set; }
        public double Discount { get; set; }
    }
    public class ProductWithSubjectAndIds
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }//所属类别编号
        public int? CategoryId { get; set; }//所属类别编号
        public string SubjectName { get; set; }
        public string ProductName { get; set; }//产品名称
        public bool State { get; set; }//产品状态
        public DateTime OverDate { get; set; }//产品下架时间
        public DateTime CreateDate { get; set; }//产品上架时间
        public double Price { get; set; }//产品价格
        public bool IsDiscountForOld { get; set; }//是否有老学员优惠k
        public double? DiscountValue { get; set; }//优惠金额
        public string AccordIdList { get; set; }//判断老学员依据的产品编号
        public bool IsPackage { get; set; }//是否是套餐类产品
        public string PackageIdList { get; set; }//判断老学员依据的产品编号
        public List<int> classIds { get; set; }
        public List<int> serviceIds { get; set; }
        public List<int> couponIds { get; set; }
        public int Sort { get; set; }//产品排序序号
    }

   public class ReceiptVM
    {
        public int Id { get; set; }
        public int PostUserId { get; set; }
        public DateTime CreateTime { get; set; }
        public ReceiptState State { get; set; }
        public DateTime? ConfirmTime { get; set; }
        public int? ConfirmerID { get; set; }
        public double Value { get; set; }
        public string PosterName { get; set; }
        public int CheckUserId { get; set; } //表示当前有查看权限的psotuser
    }


    public class OrderVMForAccount
    {
        public string StudentName { get; set; }
        public string ProductName { get; set; }
        public double ActualPay { get; set; }

        public double Debt { get; set; }
        public double Discount { get; set; }
        public DateTime OrderDate { get; set; }
        public string TradeNo { get; set; }
        public string Channel { get; set; }
    }

    public class CompensationVMFoAccount
    {
        public string StudentName { get; set; }
        public List<string> ProductNames { get; set; }
        public double Debt { get; set; }
        public double Value { get; set; }
        public DateTime FeeDate { get; set; }
    }

    public class ReceiptDetail
    {
        public List<OrderVMForAccount> Orders { get; set; }
        public List<CompensationVMFoAccount> Fees { get; set; }
    }

    public class OrderDetailVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public string SellerName { get; set; }
        public DateTime Date { get; set; }
        public double ActualPay { get; set; }
        public double Discount { get; set; }
        public double Debt { get; set; }
    }
}
  