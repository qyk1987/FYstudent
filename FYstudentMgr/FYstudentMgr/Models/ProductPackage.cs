using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class ProductPackage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product
        {
            get; set;
        }
    }
}