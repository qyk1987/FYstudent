using FYstudentMgr.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    
    public enum ActiveType{      
       Answer,
       Ask,
       BestAnswer,
       Play,
       Release,
       Commet
    }
   public enum ClientType{      
       电脑,
       手机
    }
    public class ActiveLog
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int ActiveLogID { get; set; }
        //public int LogedID { get; set; }//存放reply或者lesson或者discuss的id
        //public string ClassID { get; set; }
        public ActiveType ActiveType { get; set; }
        public DateTime LogDate { get; set; }
        public int UserID { get; set; }
        public ClientType ClientType { get; set; }
        public int Point { get;set; }
        public virtual ApplicationUser User { get; set; }


        public ActiveLog() {
            this.LogDate = DateTime.Now;
        }

        /// <summary>
        /// 记录学点值并保存该记录
        /// </summary>
        /// <param name="point"></param>
        public void Log(int point) {
            if (this.ActiveType == ActiveType.Answer || this.ActiveType == ActiveType.Ask || this.ActiveType == ActiveType.Release)
            {
                this.Point = 1;
            }
            else if (this.ActiveType == ActiveType.BestAnswer) {
                this.Point = 10;
            }
            else if (this.ActiveType == ActiveType.Play)
            {
                this.Point = Point;
            }
            else {
                this.Point = 0;
            }
           
            //db.SaveChanges();
        }
        /// <summary>
        /// 返回动态条目的标题对象
        /// </summary>
        /// <returns></returns>
        public AactiveObject GetTitleObject()
        {
            if (this.ActiveType == ActiveType.Answer||this.ActiveType == ActiveType.BestAnswer) {
               return  db.Replys.Where(r=>r.ActiveLogID==this.ActiveLogID).Single().GetObject();
            }else if(this.ActiveType == ActiveType.Ask) {
                return db.Discusss.Where(d => d.ActiveLogID == this.ActiveLogID).Single();
            }
            else if (this.ActiveType == ActiveType.Play)
            {
                return db.StudyRecords.Where(s => s.ActiveLogID == this.ActiveLogID).Single();
            }
            else {
                return db.Articles.Where(a => a.ActiveLogID == this.ActiveLogID).Single();
            }
        }



        public Reply GetReply() {
            if (this.ActiveType == ActiveType.Answer || this.ActiveType == ActiveType.BestAnswer)
            {
                 return  db.Replys.Where(r=>r.ActiveLogID==this.ActiveLogID).Single();
            }
            return null;
        }
    }
}