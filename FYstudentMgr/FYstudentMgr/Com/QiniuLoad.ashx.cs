using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.Storage;
using Qiniu.Util;


namespace FYstudentMgr.Common
{
    /// <summary>
    /// Summary description for QiniuLoad
    /// </summary>
    public class mytoken
    {
        public string uptoken = "";
    }
    public class QiniuLoad : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            string AK = "pdPNmhvjIblesAXvOD5RElGqHGbYxgAf85lm-Xwz";
            string SK = "QZsV14FLisnnDnCVwKy0YOWejZqqT7HWN5DcdjFk";
            // 目标空间名
            string bucket = "boao";
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucket;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);

            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);

            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            Console.WriteLine(JsonConvert.SerializeObject(token1));
            uploadToken = JsonConvert.SerializeObject(token1);
            context.Response.ContentType = "text/plain";
            context.Response.Write(uploadToken);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}