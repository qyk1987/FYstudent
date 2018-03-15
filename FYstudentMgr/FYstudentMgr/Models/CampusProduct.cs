using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models
{
    public class CampusProduct
    {
        public int Id { get; set; }
        public int CampusId { get; set; }//校区id
        public int ProductId { get; set; }//产品id
        public bool State { get; set; }//产品状态 是上架还是下架

    }
}