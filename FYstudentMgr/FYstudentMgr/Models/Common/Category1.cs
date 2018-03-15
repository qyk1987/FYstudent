using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using FYstudentMgr.Models.Courses;

namespace FYstudentMgr.Models.Common
{
    public class Category1
    {
        [Display(Name = "类别1")]
        public int Category1ID { get; set; }
        [Required]
        public string Catetory_1Name { get; set; }
        [Display(Name = "大科目")]
        public int SubjectID { get; set; }

        [Display(Name = "是否锁定")]
        public Boolean IsLock { get; set; }
        public int OrderIndex { get; set; }
        public virtual ICollection<Category2> Category2s { get; set; }
        public virtual Subject Subject { get; set; }
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
    //        var category = db.Category2s.Where(c => c.OrderIndex == value).Single();
    //        if (category == null)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }

    //    }
    //}
}