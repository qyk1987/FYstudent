using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dm.Model.V20151123;
using FYstudentMgr.Configurations;
using System.Configuration;
namespace FYstudentMgr.Common.Email
{
    public static class EmailHelper
    {
        //private static string accessKey;
        //private static string accessSecret;

        //static EmailHelper() {
        //    MailAccessConfig mailConfig = (MailAccessConfig)ConfigurationManager.GetSection("application/mailAccess");
        //    accessKey = mailConfig.AccessKey;
        //    accessSecret = mailConfig.AccessSecret;
        //}

        //    public static SendResult SendEmail(Email email){
        //        IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKey, accessSecret);
        //        IAcsClient client = new DefaultAcsClient(profile);
        //        SendResult result=new SendResult();
        //        SingleSendMailRequest request = new SingleSendMailRequest();
        //        try
        //        {
        //            request.AccountName = email.AccountName;
        //            request.FromAlias =email.FromAlias;
        //            request.AddressType =email.AddressType;
        //            request.TagName = email.TagName;
        //            request.ReplyToAddress =email.ReplyToAddress;
        //            request.ToAddress =email.ToAddress;
        //            request.Subject =email.Subject;
        //            request.HtmlBody =email.HtmlBody;
        //            SingleSendMailResponse httpResponse = client.GetAcsResponse(request);

        //        }
        //        catch (ServerException e)
        //        {
        //           result.Success=false;
        //           result.Message=e.Message;
        //        }
        //        catch (ClientException e)
        //        {
        //           result.Success=false;
        //           result.Message=e.Message;
        //        }
        //    }

        //    static void Main(string[] args)
        //    {
        //        MailAccessConfig mailConfig = (MailAccessConfig)ConfigurationManager.GetSection("application/mailAccess");
        //        IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAIH1OZtLGtTMWt", "ZfINVG3io4m7EuCf7ZMcBdVRqKduzU");
        //        IAcsClient client = new DefaultAcsClient(profile);
        //        SingleSendMailRequest request = new SingleSendMailRequest();
        //        try
        //        {
        //            request.AccountName = "admin@smail.xueqitian.com";
        //            request.FromAlias = "七天君";
        //            request.AddressType = 1;
        //            request.TagName = "getpwd";
        //            request.ReplyToAddress = true;
        //            request.ToAddress = "274355846@qq.com";
        //            request.Subject = "修改密码通知";
        //            request.HtmlBody = "这是正文";
        //            SingleSendMailResponse httpResponse = client.GetAcsResponse(request);
        //        }
        //        catch (ServerException e)
        //        {
        //           Console.Write( e.Message);
        //        }
        //        catch (ClientException e)
        //        {
        //            Console.Write(e.Message);
        //        }
        //    }
        //}


        //public class XqtEmail
        //{
        //   public 
        //   public bool SendEmail() {

        //    }
        //}

        //public interface Email{
        //       bool SendEmail();
        //}

        //public class SingleEmail:Email
        //{
        //   private string Action{get;set;}     
        //   private  string AccountName{get;set;}
        //   private  string FromAlias{get;set;}
        //   private  int  AddressType{get;set;}
        //   private  bool ReplyToAddress{get;set;}
        //   public   string TagName{get;set;}
        //   public  string ToAddress{get;set;}
        //   public  string Subject{get;set;}
        //   public string HtmlBody{get;set;}
        //   public bool SendEmail() {

        //    }
        //}


        //public class SendResult(){
        //   public bool Success{get;set;}
        //   public string Message{get;set;}
        //}
    }
}