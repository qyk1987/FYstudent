using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Qiniu.Common;
using Qiniu.Storage;
using Qiniu.Util;
using System.IO;
using System.Text;
using Qiniu.Http;
namespace FYstudentMgr.Controllers
{
    public class mytoken
    {
        public string uptoken = "";
    }
    [Authorize]
    public class QiniuController : ApiController
    {
        private string AK = "pdPNmhvjIblesAXvOD5RElGqHGbYxgAf85lm-Xwz";
        private string SK = "QZsV14FLisnnDnCVwKy0YOWejZqqT7HWN5DcdjFk";
        // 目标空间名
        private string bucket1 = "boao";
        private string bucket2 = "boaopublic";
        private string bucketimg = "xueqitianimg";
        private string bucketcss = "xueqitiancss";
        private string bucketupimg = "xueqitianimage";
        private string bucketvideo = "xueqitianvideo";
        private string bucketfeiyang = "feiyang";
        //供给七牛上传组件调用
        // GET: Help
        public string UpToken(string filename)
        {
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucket2 + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);

            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);

            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            uploadToken = JsonConvert.SerializeObject(token1);
            return uploadToken;
        }
        [Authorize(Roles = "Admin")]
        public string UpTokenCss(string filename)
        {
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucketupimg + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);

            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);

            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            uploadToken = JsonConvert.SerializeObject(token1);
            return uploadToken;
        }

        [Authorize(Roles = "Admin")]
        public string UpVideoToken(string filename)
        {
            string code = StringUtils.urlSafeBase64Encode("4115220123456789");
            string tsname = StringUtils.urlSafeBase64Encode(filename + "$(count)");
            string saveas_key = StringUtils.urlSafeBase64Encode(bucketvideo + ":" + filename);
            string hlsKeyUrl = StringUtils.urlSafeBase64Encode("http://www.xueqitian.com/Help/CourseVideo");
            string fops = "avthumb/m3u8/noDomain/1/vb/320k/s/1280x720/segtime/30/ab/192k/stripmeta/0/ar/22050/acodec/libfaac/r/5/hlsKey/" + code + "/hlsKeyUrl/" + hlsKeyUrl + "/pattern/" + tsname + "|saveas/" + saveas_key;//+ "/pattern/" + tsname
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            //putPolicy.PersistentOps =
            // 设置要上传的目标空间
            putPolicy.Scope = bucketvideo + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            putPolicy.PersistentOps = fops;
            putPolicy.PersistentPipeline = "xqtvedio";
            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);

            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            uploadToken = JsonConvert.SerializeObject(token1);
            return uploadToken;
        }


        public string UpVideoTokenTrial(string filename)
        {
            string code = StringUtils.urlSafeBase64Encode("4115220123456789");
            string saveas_key = StringUtils.urlSafeBase64Encode(bucketvideo + ":" + filename);
            string tsname = StringUtils.urlSafeBase64Encode(filename + "$(count)");
            string hlsKeyUrl = StringUtils.urlSafeBase64Encode("http://www.xueqitian.com/Help/lessonview");
            string fops = "avthumb/m3u8/noDomain/1/vb/320k/s/1280x720/segtime/30/ab/192k/stripmeta/0/ar/22050/acodec/libfaac/r/5/hlsKey/" + code + "/hlsKeyUrl/" + hlsKeyUrl + "/pattern/" + tsname + "|saveas/" + saveas_key;//+ "/pattern/" + tsname
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            //putPolicy.PersistentOps =
            // 设置要上传的目标空间
            putPolicy.Scope = bucketvideo + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            putPolicy.PersistentOps = fops;
            putPolicy.PersistentPipeline = "xqtvedio";
            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);

            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            uploadToken = JsonConvert.SerializeObject(token1);
            return uploadToken;
        }
       
        public string UpTokenimg(string filename)
        {
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucketupimg + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            //return putPolicy.Scope;


            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);
            mytoken token1 = new mytoken();
            token1.uptoken = uploadToken;
            uploadToken = JsonConvert.SerializeObject(token1);
            return uploadToken;

        }

        [AllowAnonymous]
        public string UpTokenNew()
        {
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            var ret = new
            {
                url = "$(key)",
                key = "$(key)",
                w = "$(imageInfo.width)",
                h = "$(imageInfo.height)",
                state = "SUCCESS"
            };

            putPolicy.ReturnBody = JsonConvert.SerializeObject(ret);

            // 设置要上传的目标空间
            putPolicy.Scope = bucketupimg;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);
            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);


            return uploadToken;
        }
        // GET: Help
        public string DownToken(string url)
        {
            string rawURL = url + "?e=" + GetTimeStamp();
            Mac mac = new Mac(AK, SK);
            string token = Auth.createDownloadToken(rawURL, mac);
            string signedURL = rawURL + "&token=" + token;
            return signedURL;
        }

        // GET: Help
        public string DownVideo(string url)
        {
            string rawURL = url + "?pm3u8/0/expires/43200&e=" + GetTimeStamp();
            //string rawURL = url + "?e=" + GetTimeStamp();
            Mac mac = new Mac(AK, SK);
            string token = Auth.createDownloadToken(rawURL, mac);
            string signedURL = rawURL + "&token=" + token;
            return signedURL;
        }
        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds + 10).ToString();
        }




        public string UpTokenAvatar(string filename)
        {
            // 上传策略
            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
            PutPolicy putPolicy = new PutPolicy();
            // 设置要上传的目标空间
            putPolicy.Scope = bucketimg + ":" + filename;
            // 上传策略的过期时间(单位:秒)
            putPolicy.SetExpires(3600);

            //return putPolicy.Scope;
            Mac mac = new Mac(AK, SK); // Use AK & SK here
            // 生成上传凭证
            string uploadToken = Auth.createUploadToken(putPolicy, mac);


            return uploadToken;
        }
        //public void Avatar() { 
        //    //String pic = Request["pic"]; 
        //    try {
        //        String pic1 = Request["pic1"];
        //        String pic2 = Request["pic2"];
        //        int userid = User.Identity.GetUserId<int>();
        //        string saveKey1 = "avatar/" + userid + ".jpg";
        //        string saveKey2 = "avatar50/" + userid + ".jpg";
        //        if (pic1.Length > 0)
        //        {
        //            byte[] bytes1 = Convert.FromBase64String(pic1);
        //            byte[] bytes2 = Convert.FromBase64String(pic2);
        //            UploadOptions uploadOptions = null;
        //            // 上传完毕事件处理
        //            Qiniu.Common.Config.ZONE = Qiniu.Common.Zone.ZONE_CN_South();
        //            UpCompletionHandler uploadCompleted = new UpCompletionHandler(OnUploadCompleted);
        //            FormUploader fu = new FormUploader();
        //            fu.uploadData(bytes1, saveKey1, UpTokenAvatar(saveKey1), uploadOptions, uploadCompleted);
        //            fu.uploadData(bytes2, saveKey2, UpTokenAvatar(saveKey2), uploadOptions, uploadCompleted);
        //            ApplicationDbContext db = new ApplicationDbContext();
        //            var student=db.Users.Find(userid).Student;
        //            if (!student.IsUploaImg) {
        //                student.IsUploaImg = true;
        //                student.XP = student.XP + 2;
        //                db.SaveChanges();
        //            }
        //            Console.Write("ok");
        //        }

        //    }catch(Exception e){
        //        Console.Write(e.Message);
        //    }

        //String pic3 = Request["pic3"]; 
        //原图 
        //if (pic.Length == 0)
        //{
        //}
        //else
        //{
        //    byte[] bytes = Convert.FromBase64String(pic);  //将2进制编码转换为8位无符号整数数组

        //    string url = "./src.png";
        //    FileStream fs = new FileStream(context.Server.MapPath(url), System.IO.FileMode.Create);
        //    fs.Write(bytes, 0, bytes.Length);
        //    fs.Close();
        //}
        //byte[] bytes1 = Convert.FromBase64String(pic1);  //将2进制编码转换为8位无符号整数数组.
        //byte[] bytes2 = Convert.FromBase64String(pic2);  //将2进制编码转换为8位无符号整数数组.
        //byte[] bytes3 = Convert.FromBase64String(pic3);  //将2进制编码转换为8位无符号整数数组.
        //图1，
        //string url1 = "./1.png";//需要修改图片保存地址，否则每次都是1.png，第二次会覆盖，为避免重名，可以使用guid：string fileLoadName =Guid.NewGuid().ToString() + ".png";
        //FileStream fs1 = new FileStream(context.Server.MapPath(url1), System.IO.FileMode.Create);
        //fs1.Write(bytes1, 0, bytes1.Length);
        //fs1.Close();
        ////图2
        //string url2 = "./2.png";
        //FileStream fs2 = new FileStream(context.Server.MapPath(url2), System.IO.FileMode.Create);
        //fs2.Write(bytes2, 0, bytes2.Length);
        //fs2.Close();
        ////图3
        //string url3 = "./3.png";
        //FileStream fs3 = new FileStream(context.Server.MapPath(url3), System.IO.FileMode.Create);
        //fs3.Write(bytes3, 0, bytes3.Length);
        //fs3.Close();
        ////这里响应的是1，前台接收到json数组{status:1}，如果返回图片地址，如改为context.Response.Write("{/"status/":"+url1+"}");则前台页面无法执行uploadevent方法，只能按固定格式{/"status/":1}。
        ////如果想返回图片路径，可以用静态类或session等方式。
        //context.Response.Write("{/"status/":1}");

        //}

        //public string CourseVideo()
        //{
        //    Response.AppendHeader("Access-Control-Allow-Origin", "*");
        //    return "4115220123456789";
        //}
        //[AllowAnonymous]
        //public string lessonview()
        //{
        //    Response.AppendHeader("Access-Control-Allow-Origin", "*");//设置跨域访问

        //    return "4115220123456789";
        //}

        private void OnUploadCompleted(string key, ResponseInfo respInfo, string respJson)
        {
            //return "";
        }
    }
}
