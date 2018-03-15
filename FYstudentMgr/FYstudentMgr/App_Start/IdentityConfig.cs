using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using FYstudentMgr.Models;
using System;
using System.Data.Entity;
using System.Web;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Text;
using System.Net.Mail;
using FYstudentMgr.Configurations;
using System.Configuration;
using FYstudentMgr.Common.Sms;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
namespace FYstudentMgr
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser, string>
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ApplicationUserManager(IUserStore<ApplicationUser, string> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, string>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider("电话代码", new PhoneNumberTokenProvider<ApplicationUser, string>
            {
                MessageFormat = "你的安全代码是{0}"
            });
            manager.RegisterTwoFactorProvider("电子邮件代码", new EmailTokenProvider<ApplicationUser, string>
            {
                Subject = "安全代码",
                BodyFormat = "你的安全代码是 {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser, string>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        public async Task<ApplicationUser> FindByPhoneNumberAsync(string PhoneNumber)
        {
            return await db.Users.SingleOrDefaultAsync(u => u.PhoneNumber == PhoneNumber);

        }
        public ApplicationUser FindByPhoneNumber(string PhoneNumber)
        {
            return db.Users.Where(u => u.PhoneNumber == PhoneNumber).FirstOrDefault();

        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();

            }

            base.Dispose(disposing);
        }
    }
    public class ApplicationRoleManager : RoleManager<ApplicationRole, string>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        }
    }


    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            MailConfig mailConfig = (MailConfig)ConfigurationManager.GetSection("application/mail");
            if (mailConfig.RequireValid)
            {
                // 设置邮件内容
                var mail = new MailMessage(
                    new MailAddress(mailConfig.Uid, "学七天教育"),
                    new MailAddress(message.Destination)
                    );
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                // 设置SMTP服务器
                var smtp = new SmtpClient(mailConfig.Server, mailConfig.Port);
                smtp.Credentials = new System.Net.NetworkCredential(mailConfig.Uid, mailConfig.Pwd);

                await smtp.SendMailAsync(mail);
            }
            await Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            SmsMessage msg = (SmsMessage)message;
            StringBuilder arge = new StringBuilder();
            arge.AppendFormat("Account={0}", "15307000371");
            arge.AppendFormat("&Pwd={0}", "f3cf3ac5c5ffac5b51ff2771f");
            arge.AppendFormat("&Content={0}", msg.Body);
            arge.AppendFormat("&Mobile={0}", msg.Destination);
            arge.AppendFormat("&SignId={0}", 33533);
            arge.AppendFormat("&TemplateId={0}", msg.TemplateId);
            //arge.AppendFormat("&SendTime={0}", request.SendTime);
            string weburl = "http://api.feige.ee/SmsService/Template";
            string resp = SmsHelper.PushToWeb(weburl, arge.ToString(), Encoding.UTF8);
            // Plug in your sms service here to send a text message.
            try
            {
                SendSmsResponse response = JsonConvert.DeserializeObject<SendSmsResponse>(resp);
                if (response.Code == 0)
                {
                    //成功
                    return Task.FromResult(0);
                }
                else
                {
                    //失败
                    return Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                //记录日志
                return Task.FromResult(0);
            }

        }

    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ApplicationDbContext db)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "admin@example.com";
            const string password = "Admin@123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName); ;
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = name };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }
        }
    }

    //public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    //{
    //    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
    //        base(userManager, authenticationManager)
    //    { }

    //    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    //    {
    //        return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    //    }

    //    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    //    {
    //        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    //    }
    //}
}
