using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class ProductService
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ServiceId { get; set; }
        public int Sort { get; set; }

        public virtual Product Product { get; set; }
        public virtual Service Service { get; set; }
    }
}