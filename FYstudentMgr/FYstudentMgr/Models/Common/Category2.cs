using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FYstudentMgr.Models.Common
{   
    public class Category2
    {
        [Display(Name = "类别2")]
        public int Category2ID { get; set; }
        [Display(Name = "类别1")]
        public int Category1ID { get; set; }

        [Display(Name = "是否锁定")]
        public Boolean IsLock { get; set; }
        
        public int OrderIndex { get; set; }
        [Required]
        public string Catetory_2Name { get; set; }
        public virtual Category1 Category1 { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Discuss> Discusses { get; set; }
    }

    ///// <summary>
    ///// 自定义unique唯一验证
    ///// </summary>
    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    //public class UniqueAttribute : ValidationAttribute
    //{
    //    public override Boolean IsValid(int value)
    //    {
    //        ApplicationDbContext db = new ApplicationDbContext();
    //       var category= db.Category1s.Where(c => c.OrderIndex == value).Single();
    //       if (category == null)
    //       {
    //           return true;
    //       }
    //       else {
    //           return false;
    //       }
            
    //    }
    //}
}