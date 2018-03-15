using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Common.Sms
{
    public class BaseCode
    {
        //状态码
        public int Code { get; set; }
        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string Message { get; set; }
    }
}