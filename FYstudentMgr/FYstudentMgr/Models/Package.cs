using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateTime { get; set; }
        public double DiscountValue { get; set; }
        public bool State { get; set; }//套餐上下架

        public virtual ICollection<ProductPackage> Products { get; set; }
    }
}