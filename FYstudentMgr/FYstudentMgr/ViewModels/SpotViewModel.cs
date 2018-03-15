using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class SpotViewModel
    {
    }
    public class SpotView
    {
        public int Id { get; set; }
        public string SpotName { get; set; }//服务点名称
        public string SpotAddress { get; set; }//服务点地址
        public bool SpotState { get; set; }//服务点状态
        public int CampusID { get; set; }//所属校区ID
        public DateTime CreateDate { get; set; }
        public int DistrictID { get; set; }
    }
    public class TreeNode
    {
        public string name { get; set; }
        public double value { get; set; }
        public List<TreeNode> children { get; set; }
        public int? id { get; set; }
    }

    public class TreeItem
    {

        public int Id { get; set; }

        public List<TreeItem> items { get; set; }

    }
}