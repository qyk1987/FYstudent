using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }//所属类别编号
        public string ProductName { get; set; }//产品名称
        public string Desc { get; set; }//商品描述
        public bool State { get; set; }//产品状态
        public DateTime? OverDate { get; set; }//产品下架时间
        public DateTime CreateDate { get; set; }//产品上架时间
        public double Price { get; set; }//产品价格
        public bool IsDiscountForOld { get; set; }//是否有老学员优惠k
        public double? DiscountValue { get; set; }//优惠金额
        public string AccordIdList { get; set; }//判断老学员依据的产品编号
        public bool IsPackage { get; set; }//是否是套餐类产品
        public string PackageIdList { get; set; }//该产品包含的产品id列表
        public int Sort { get; set; }//产品排序序号
        public string CoverImg { get; set; }//封面图片
        public int SaleCount { get; set; }
        public List<int> couponIds { get; set; }
        public List<int> serviceIds { get; set; }
    }

    public class CouponVM
    {
        public int Id { get; set; }
        public string CouponName { get; set; }//优惠名称
        public double Vlaue { get; set; }//优惠金额
        public string Rule { get; set; }//使用规则
        public DateTime StartDate { get; set; }//开始时间
        public DateTime OverDate { get; set; }//结束时间
        public bool State { get; set; }//优惠政策状态  是否可用
        public List<int> productIds { get; set; }
        public List<int> campusIds { get; set; }
    }

    public class CategoryWithCount
    {
       public int Id { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
  
    }
    public class SubjectWithCount
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int Count { get; set; }

    }
    public class ProductWithCount
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }

    }

    public class SimpleProduct
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }//所属类别编号
        public string ProductName { get; set; }//产品名称

    }

}