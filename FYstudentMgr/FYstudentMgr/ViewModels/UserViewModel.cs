using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.ViewModels
{
    public class UserViewModel
    {
    }

    public class UserView
    {
        public string  Id { get; set; }
        public bool IsUploaImg { get; set; }
        public string Img { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {get;set;}
        public string UserName { get; set; }
    }
}
