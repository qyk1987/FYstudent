using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public bool State { get; set; }
        public int Sort { get; set; }//类别排序
        public DateTime CreateTime { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}