using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FYstudentMgr.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public bool HasRegistered { get; set; }

        public string LoginProvider { get; set; }
    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class Auth
    {

        public UserInfo user { get; set; }


        public string userId { get; set; }
        

        
        
        public string currentDutyId { get; set; }
        public Dictionary<string, Duty> dutys { get; set; }

    }

    public class Duty
    {
        public int Id { get; set; }
        public int PostId { get; set; }//所属岗位id
        public string RoleId { get; set; }
        public string PostName { get; set; }
        public int? DistrictId { get; set; }
        public int? CampusId { get; set; }
        public int? SpotId { get; set; }
        public string RoleName { get; set; }
        public string RoleLabel { get; set; }
        public string DistrictName { get; set; }
        public string CampusName { get; set; }
        public string SpotName { get; set; }
        public List<int> Ids { get; set; }


    }

    public class UserInfo
    {
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ErrorResult
    {
        public string error { get; set; }
        public string error_description { get; set; }

        public string tostring()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class VerifyResult
    {
        public string number { get; set; }
        public string token { get; set; }
    }
}
