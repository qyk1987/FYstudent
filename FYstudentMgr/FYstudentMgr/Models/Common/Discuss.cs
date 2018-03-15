using FYstudentMgr.Common;
using FYstudentMgr.Models.Courses;
using FYstudentMgr.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public class Discuss : ReplayObject
    {
        private ApplicationDbContext db = new ApplicationDbContext();
       
        [Display(Name = "问答编号")]
        public int DiscussID { get; set; }
       
        [Display(Name = "作者")]
        public int UserID { get; set; }
        [Display(Name = "问答主题")]
        public int CourseID { get;set;}
        public int? LessonID { get; set; }
        public int? QuestionID { get; set; }
        public int? ExerciseID { get; set; }
        [Display(Name = "问答标题")]
        [Required]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "问答内容")]      
        public string Content { get; set; }
        [Display(Name = "浏览次数")]
        public int ReadTimes{ get; set; }
        [Display(Name = "被赞次数")]
        public int LikeTimes { get; set; }
        [Display(Name = "回复次数")]
        public int ReplyTimes { get; set; }
        [Display(Name = "最新回复")]
        public int? LatestReplyID { get; set; }
        public int? BestReplyID { get; set; }
        [Display(Name = "发布时间")]
        public DateTime CreateTime { get; set; }
        public int ActiveLogID { get; set; }
        public int ClassID { get; set; }
        public int Category2ID { get; set; }
        public Boolean IsCHeck { get; set; }
        public virtual Category2 Category2 { get; set; }
        [ForeignKey("UserID")]       
        public virtual ApplicationUser User { get; set; }
        public virtual Class Class { get; set; }
        public virtual Course Course { get; set; }
        public virtual Lesson Lesson { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual Question Question { get; set; }
        public virtual ActiveLog ActiveLog { get; set; }
        public virtual Reply LatestReply { get; set; }
        public void AddRead() {
            this.ReadTimes = this.ReadTimes + 1;
        }

        public Discuss()
        {
            this.ReadTimes = 0;
            this.LikeTimes = 0;
            this.CreateTime = DateTime.Now;
            //this.User = new ApplicationDbContext().Users.Single(u => u.User_ID == this.UserID);
        }

         public void AddLike()
        {
            this.LikeTimes = this.LikeTimes + 1;
        }

        public void CancelLike()
        {
            this.LikeTimes = this.LikeTimes - 1;
        }
        /// <summary>
        /// 返回该问题的所有回复
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Reply> GetReplys()
        {
            var replays = db.Replys.Where(r => r.ReplyID == this.DiscussID)
                        .Where(r => r.ReplyType == ReplyType.Discuss)
                        .OrderBy(r => r.CreateTime);
            return replays.ToList().Concat(replays.ToList().SelectMany(t => GetSonID(t.ID)));
        }
        /// <summary>
        /// 获取某回复下的所有子回复
        /// </summary>
        /// <param name="p_id"></param>
        /// <returns></returns>
        private IEnumerable<Reply> GetSonID(int p_id)
        {
            var query = from c in db.Replys
                        where c.ReplyID == p_id && c.Level > 1
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
        }

        /// <summary>
        /// 获取该问题的所有相关问题
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Discuss> GetSimilarDiscss() {
            var discusses = db.Discusss.Where(d => d.LessonID == this.LessonID).OrderByDescending(d => d.ReplyTimes).Take(20);
            return discusses.ToList();
        }


        /// <summary>
        /// 实现继承的AactiveObject接口的GetTitle方法
        /// </summary>
        /// <returns></returns>
        public string GetTitle() {
            return this.Title;
        }

        public int GetID()
        {
            return this.DiscussID;
        }
        public string GetTypeName()
        {
            return "Discuss";
        }

        public int GetClassID()
        {
            return this.ClassID;
        }

       

        public int GetSubjectID() {
            return this.Category2.Category1.SubjectID;
        }
    }
}