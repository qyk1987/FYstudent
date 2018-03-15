using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using FYstudentMgr.Models;
using System.Text.RegularExpressions;
using FYstudentMgr.Common;
using FYstudentMgr.Common.Sms;
using System.Data.Entity;
using System.Data.SqlClient;

namespace FYstudentMgr.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        
        // GET: /Manage/Index
        /// <summary>
        /// 账号安全主页面
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<ActionResult> Secure(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "密码修改成功！"
                : message == ManageMessageId.SetPasswordSuccess ? "密码已设定"
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "出现了一个错误"
                : message == ManageMessageId.AddPhoneSuccess ? "电话号码已添加"
                : message == ManageMessageId.RemovePhoneSuccess ? "电话号码已移除"
                : message == ManageMessageId.ChangePhoneSuccess ? "电话号码修改成功"
                : "";

            var userId = User.Identity.GetUserId<int>();
            Regex re = new Regex(@"(\d{3})(\d{4})(\d{4})", RegexOptions.None);
            string pattern = "\\w{3}(?=@\\w+?.com)";
            var number=await UserManager.GetPhoneNumberAsync(userId);
            var email=await UserManager.GetEmailAsync(userId);
            if(number!=null){
                number = re.Replace(number, "$1****$3");
            }
            if (email != null) {
                email = Regex.Replace(email, pattern, "***");
            }
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber =number,
                Email =email, //Regex.Replace(await UserManager.GetEmailAsync(userId), pattern, "***"),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId.ToString())
            };
            return View(model);
        }


        
        //public async Task<ActionResult> Secure()
        //{
        //    var id = User.Identity.GetUserId<int>();
        //    var user = db.Users.Find(id);
        //    return View(user);
        //}


        /// <summary>
        /// 信息管理主页面
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            ViewBag.HasPassword = HasPassword();
            ViewBag.HasPhoneNumber = HasPhoneNumber();
            ViewBag.HasEmail = HasEmail();
            return View(user);
        }


        public ActionResult OrderList()
        {
            
            return View();
        }

        //public async Task<ActionResult> Task()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId<int>());
        //    var model = new TaskViewModel()
        //    {
        //        HasEmail = HasEmail(),
        //        HasImage = HasImage(),
        //        HasWeiXin = HasWeiXin(),
        //        HasPhoneNumber = HasPhoneNumber(),
        //        HasQQ = HasQQ()
        //    };
        //    return PartialView(model);
        //}

        /// <summary>
        /// 基本信息页面
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Info()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());

            return View(user);
        }

        /// <summary>
        /// 找回密码主页面
        /// </summary>
        /// <returns></returns>
        public ActionResult PwdReset()
        {
            
            ViewBag.HasPhoneNumber = HasPhoneNumber();
            ViewBag.HasEmail = HasEmail();
            return View();
        }
        /// <summary>
        /// 验证邮箱重置密码
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PwdResetEmail()
        {
            var userId = User.Identity.GetUserId<int>();
            string email=await UserManager.GetEmailAsync(userId);
            string pattern = "\\w{3}(?=@\\w+?.com)";
            string result = Regex.Replace(email, pattern, "***");
            ViewBag.Email = result;
            return View();
        }
        /// <summary>
        /// ajax发送验证邮件
        /// </summary>
        /// <returns></returns>
        public async Task<string> SendEmail(string code)
        {
            
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (code == null) { return "no-code"; }
            if (SessionHelper.GetSession("verifycode") == null || code.ToLower() != SessionHelper.GetSession("verifycode").ToString())
            {                   
                return "code-error"; 
            }                       
             var email = await UserManager.GetEmailAsync(user.Id);
             try
             {
                 var ecode = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                 var callbackUrl = Url.Action("ConfirmEmail", "manage", new { userId = user.Id, code = ecode }, protocol: Request.Url.Scheme);
                 await UserManager.SendEmailAsync(user.Id, "修改密码通知", "您在学七天的账号" + user.UserName + "申请修改密码,点击以下链接前往修改<br>" + callbackUrl + "<br>(如果您无法点击此链接，请将它复制到浏览器地址栏后访问)<br>如确认非本人操作，请忽略此邮件，由此给您带来的不便请谅解！");
                 return "ok";
             }
             catch (Exception e)
             {
                 return e.Message;
             }                    
        }

       /// <summary>
       /// 邮件验证后重置密码页面
       /// </summary>
       /// <param name="userId"></param>
       /// <param name="code"></param>
       /// <returns></returns>
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (User.Identity.GetUserId<int>() != userId)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }

            if (userId < 0 || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                try
                {
                    var resetcode = await UserManager.GeneratePasswordResetTokenAsync(userId);
                    ViewBag.Code = resetcode;
                    return View();
                }
                catch (Exception e)
                {
                    AddErrors(new IdentityResult(e.Message));
                }

            }
            return View("Error");
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public async Task<string> ResetEmailStep2(ResetPwdViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return "error";
            }
            var result = await UserManager.ResetPasswordAsync(User.Identity.GetUserId<int>(), model.Code, model.NewPassword);
            if (result.Succeeded)
            {
                return "ok";
            }
            // If we got this far, something failed, redisplay form
            return "error";

        }
        /// <summary>
        /// 用手机验证找回密码
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PwdResetMobile()
        {
            var userId = User.Identity.GetUserId<int>();
            string phoneNumber = await UserManager.GetPhoneNumberAsync(userId);
            if (phoneNumber == null) {
               //return View("Error");
                phoneNumber = "15307000371";
            }
            Regex re = new Regex(@"(\d{3})(\d{4})(\d{4})", RegexOptions.None);
            string result = re.Replace(phoneNumber, "$1****$3");      
            ViewBag.PhoneNumber = result;
            return View();
        }


        /// <summary>
        /// 验证用户输入的图片验证码并给用户发短信验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="number"></param>
        /// <returns></returns>
    
        public async Task<string> VerifyCode(string code,string number)
        {
            if (SessionHelper.GetSession("verifycode") == null || code.ToLower() != SessionHelper.GetSession("verifycode").ToString()) {
                return "codeerror";
            }
            int userId = 0;
            if (User.Identity.IsAuthenticated) {
                userId = User.Identity.GetUserId<int>();
            }
            

            if (number == null)
            {
                number = await UserManager.GetPhoneNumberAsync(userId);
            }
            else 
            {
                var user =await UserManager.FindByPhoneNumberAsync(number);
                if (user != null) {
                    return "found";
                }
            }                       
            try {             
                // Generate the token and send it
                var smscode = await UserManager.GenerateChangePhoneNumberTokenAsync(userId, number);
                if (UserManager.SmsService != null)
                { 
                    var message = new SmsMessage
                    {
                        Destination = number,
                        Body = smscode,
                        TemplateId=30773
                    };
                    message.setRecord(userId, number);
                    db.SmsRecords.Add(message.Record);
                    await db.SaveChangesAsync();
                    await UserManager.SmsService.SendAsync(message);
                    return "ok";// return smscode;//
                }
                else {
                    return "error";
                }
                //发送短信
                
            }catch(SqlException e){
                return e.Message;
            }
        } 


        /// <summary>
        /// 通过短信验证来重置密码时点击第一步
        /// </summary>
        /// <param name="msgcode"></param>
        /// <returns></returns>
        public async Task<string> ResetMobileStep1(string msgcode) {
            var userId = User.Identity.GetUserId<int>();
            string phoneNumber = await UserManager.GetPhoneNumberAsync(userId);
            var result = await UserManager.VerifyChangePhoneNumberTokenAsync(userId, msgcode, phoneNumber); //验证手机验证码
            var code=await UserManager.GeneratePasswordResetTokenAsync(userId); //生成重置密码token
            if (result==true)
            {
                return "code"+code;
            }
            // If we got this far, something failed, redisplay form
            return "code" + code;// return "ok";
        }

        /// <summary>
        /// 短信验证码验证通过后输入密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ResetMobileStep2(ResetPwdViewModel model)
        {
             if (!ModelState.IsValid)
            {
                return "error";
            }           
            var result = await UserManager.ResetPasswordAsync(User.Identity.GetUserId<int>(),model.Code,model.NewPassword);
            if (result.Succeeded)
            {
                return "ok";
            }
            // If we got this far, something failed, redisplay form
            return "error";
        } 
        public async Task<ActionResult> VerifyEmail()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            ViewBag.HasPassword = HasPassword();
            ViewBag.HasPhoneNumber = HasPhoneNumber();
            ViewBag.HasEmail = HasEmail();
            return View(user);
        }
        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        /// <summary>
        /// 添加电话号码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Secure", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "短信验证码错误");
            return View(model);
        }


        // GET: /Manage/AddPhoneNumber
        public async Task<ActionResult> ChangePhoneNumber()
        {
            var userId = User.Identity.GetUserId<int>();
            string phoneNumber = await UserManager.GetPhoneNumberAsync(userId);
            if (phoneNumber == null)
            {
                return View("Error");             
            }
            Regex re = new Regex(@"(\d{3})(\d{4})(\d{4})", RegexOptions.None);
            string result = re.Replace(phoneNumber, "$1****$3");
            ViewBag.PhoneNumber = result;
            return View();
        }

        /// <summary>
        /// 更换手机绑定时ajax验证短信验证码来验证原来的手机号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> VerifyPhoneNumber(string code,string number)
        {
            var userId = User.Identity.GetUserId<int>();
            if (number == null)
            {
                number=await UserManager.GetPhoneNumberAsync(userId);
            }
            if (code == null || number==null)
            {
                return "error";
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), number, code);
            if (result.Succeeded)
            {
                return "ok";
            }
            return "faild";
        }

        public ActionResult EmailBind()
        {
            if (HasEmail())
            {
                return View("Error");
            }
            return View();
        }
       
        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public ActionResult VerifyPhoneNumber()
        {
            //var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);
            //// Send an SMS through the SMS provider to verify the phone number
            //return  View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
            return View();
        }

        ////
        //// POST: /Manage/VerifyPhoneNumber
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
        //    if (result.Succeeded)
        //    {
        //        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
        //        if (user != null)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //        }
        //        return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
        //    }
        //    // If we got this far, something failed, redisplay form
        //    ModelState.AddModelError("", "Failed to verify phone");
        //    return View(model);
        //}

        //
        // GET: /Manage/RemovePhoneNumber
        //public async Task<ActionResult> RemovePhoneNumber()
        //{
        //    var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);
        //    if (!result.Succeeded)
        //    {
        //        return RedirectToAction("Index", new { Message = ManageMessageId.Error });
        //    }
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
        //    if (user != null)
        //    {
        //        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //    }
        //    return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        //}

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Secure", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }




        //public async Task<string> ModifyInfo( string name,string signature,string birthday,int sex) {
        //    var student = db.Users.Find(User.Identity.GetUserId<int>()).Student;
            
        //    try {
        //        if (name != "") {
        //            student.Name = name;
        //        }
        //        if (signature != "") {
        //            student.Signature = signature;
        //        }
        //        if (birthday != "") {
        //            student.BirthDate = DateTime.Parse(birthday);
        //        }
        //        student.studentSex = (Sex)sex;
        //        //db.Entry(student).State = EntityState.Modified;
        //       await db.SaveChangesAsync();
        //       return "ok";
        //    }catch(Exception e){
        //        return e.Message;
        //    }
        //}
        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        //// POST: /Manage/SetPassword
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
        //            if (user != null)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //            }
        //            return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // GET: /Manage/ManageLogins
        //public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        //{
        //    ViewBag.StatusMessage =
        //        message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
        //        : message == ManageMessageId.Error ? "An error has occurred."
        //        : "";
        //    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
        //    if (user == null)
        //    {
        //        return View("Error");
        //    }
        //    var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>());
        //    var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
        //    ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
        //    return View(new ManageLoginsViewModel
        //    {
        //        CurrentLogins = userLogins,
        //        OtherLogins = otherLogins
        //    });
        //}

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new Account2Controller.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId<int>().ToString());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId<int>().ToString());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                db.Dispose();
                _userManager.Dispose();
                _userManager = null;
            }
           
            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                if (error == "Incorrect password.")
                {
                    ModelState.AddModelError("", "原登录密码错误");
                }
                else if (error.StartsWith("Passwords must have at least one lowercase"))
                {
                    ModelState.AddModelError("", "密码至少要包含一个小写字母");
                }
                else if (error.StartsWith("Passwords must have at least one digit"))
                {
                    ModelState.AddModelError("", "密码至少要包含一个数字");
                }
                else
                {
                    ModelState.AddModelError("", error);
                }
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }
        private bool HasEmail()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.Email != null;
            }
            return false;
        }

        //private bool HasQQ()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId<int>());
        //    if (user != null)
        //    {
        //        return user.Student.QQ!=null;
        //    }
        //    return false;
        //}

        //private bool HasWeiXin()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId<int>());
        //    if (user != null)
        //    {
        //        return user.Student.WeiXin_ID != null;
        //    }
        //    return false;
        //}

        //private bool HasImage()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId<int>());
        //    if (user != null)
        //    {
        //        return user.Student.IsUploaImg;
        //    }
        //    return false;
        //}

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            ChangePhoneSuccess,
            Error
        }

#endregion
    }
}