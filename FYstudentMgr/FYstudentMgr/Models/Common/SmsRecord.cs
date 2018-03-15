using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public enum SmsCate{
        注册账户,忘记密码,绑定手机,修改密码,购买课程,充值学币,公告消息,监督学习
    }
    public class SmsRecord
    {
        public int SmsRecordID { get; set; }
        public int UserID { get; set; }
        public SmsCate Cate { get; set; }
        public DateTime SendTime { get; set; }
        public string Number { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}