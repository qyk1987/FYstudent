using FYstudentMgr.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Common.Sms
{
    public class SmsMessage:IdentityMessage
    {
        public int TemplateId { get; set; }
        public SmsRecord Record { get; set; }


        public void setRecord(string uid,string number)
        {
            this.Record = new SmsRecord();
            switch (this.TemplateId)
            {
                case 30742: this.Record.Cate = SmsCate.购买课程; break;
                case 30741: this.Record.Cate = SmsCate.注册账户; break;
                case 30582: this.Record.Cate = SmsCate.修改密码; break;
                case 30771: this.Record.Cate = SmsCate.监督学习; break;
                case 30772: this.Record.Cate = SmsCate.忘记密码; break;
                case 30773: this.Record.Cate = SmsCate.绑定手机; break;
                case 30774: this.Record.Cate = SmsCate.充值学币; break;
            }
            this.Record.SendTime = DateTime.UtcNow;
            this.Record.UserID = uid;
            this.Record.Number = number;          
        }
    }
}