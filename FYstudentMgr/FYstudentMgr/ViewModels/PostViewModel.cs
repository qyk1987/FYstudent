using FYstudentMgr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class PostViewModel
    {
        public Post Post { get; set; }
        public int PostId { get; set; }
    }
    public class SimplePost
    {
        public int Id { get; set; }
        public string PostName { get; set; }
        public string RoleId { get; set; }
    }
}