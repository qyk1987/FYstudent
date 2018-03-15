using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }//类别名臣
        public bool State { get; set; }//类别状态
        public int Sort { get; set; }//类别排序

        public virtual ICollection<Subject> Subjects { get; set; }
    }
}