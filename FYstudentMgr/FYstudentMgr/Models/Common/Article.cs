using FYstudentMgr.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    
    public class Article : ReplayObject
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        [Display(Name = "资讯编号")]
        public int ArticleID { get; set; }
        [Display(Name = "作者")]
        public int UserID { get; set; }
       
        [Display(Name = "资讯标题")]
        [Required(ErrorMessage = "文章标题必须填写")]
        [StringLength(20, ErrorMessage = "最多只能输入20个字")]
        public string Title { get; set; }
        [Display(Name = "资讯类容")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "文章内容必须填写")]
        public string Content { get; set; }
        [Display(Name = "发布时间")]
        public DateTime CreateTime { get; set; }
        public bool IsTop { get; set; }
        public bool IsEssential { get; set; }
        [Display(Name = "浏览次数")]
        public int ReadTimes{ get; set; }
        [Display(Name = "被赞次数")]
        public int LikeTimes { get; set; }
        [Display(Name = "回复次数")]
        public int ReplyTimes { get; set; }
        [Display(Name = "最新回复")]
        public int? LatestReplyID { get; set; }
        public int ActiveLogID { get; set; }
        [Display(Name = "类别2")]
        public int Category2ID { get; set; }
        public bool IsShow { get; set; }
        public Boolean IsCHeck { get; set; }
        public virtual Category2 Category2 { get; set; }
        public virtual ActiveLog ActiveLog { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Reply LatestReply { get; set; }
        public void AddLike() {
            this.LikeTimes = this.LikeTimes + 1;
        }

        public void CancelLike()
        {
            this.LikeTimes = this.LikeTimes - 1;
        }
        public void AddRead() {
            this.ReadTimes = this.ReadTimes + 1;
        }

        public Article() {
            this.ReadTimes = 0;
            this.LikeTimes = 0;
            this.CreateTime = DateTime.Now;
        }

        public string GetTitle()
        {
            return this.Title;
        }
        public int GetID()
        {
            return this.ArticleID;
        }
        /// <summary>
        /// 返回该文张下所有的评论（包含回复）
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Reply> GetReplys()
        {
            var replays = db.Replys.Where(r => r.ReplyID == this.ArticleID)
                        .Where(r => r.ReplyType == ReplyType.Article)
                        .OrderBy(r => r.CreateTime);
            return replays.ToList().Concat(replays.ToList().SelectMany(t => GetSonID(t.ID)));
        }


        ///// <summary>
        ///// 返回该文章的评论分页数据（不包含评论的回复）
        ///// </summary>
        ///// <param name="page">页码</param>
        ///// <param name="pagesize">分页大小</param>
        ///// <returns></returns>
        //public IEnumerable<Reply> GetComments(int page,int pagesize)
        //{
        //    var replays = db.Replys.Where(r => r.ReplyID == this.ArticleID)
        //                .Where(r => r.ReplyType == ReplyType.Article)
        //                .OrderBy(r => r.CreateTime)
        //                .Skip((page-1)*pagesize)
        //                .Take(pagesize);
        //    return replays.ToList();
        //}

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

        public string GetTypeName()
        {
            return "Article";
        }

        public int  GetClassID()
        {
            return 0;
        }
       
        public int GetSubjectID()
        {
            return this.Category2.Category1.SubjectID;
        }
        protected  void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            //base.Dispose(disposing);
        }

    }
}