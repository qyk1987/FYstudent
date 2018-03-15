using FYstudentMgr.Models.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FYstudentMgr.Models;
using System.ComponentModel.DataAnnotations.Schema;
namespace FYstudentMgr.Models.Common
{
    
    public enum TopicType
    {
       Lesson,
       Article,
       Notes,
       Question
       
    }
    public class Comment
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Display(Name = "评论编号")]
        public int CommentID { get; set; }

        public int TopicID { get; set; }

        [Display(Name = "主题类型")]
        public TopicType TopicType { get; set; }

        [Display(Name = "作者")]
        public int UserID { get; set; }
        [Display(Name = "评论时间")]
        public DateTime CreateTime { get; set; }
        [Required]
        [Display(Name = "评论内容")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [Display(Name = "被赞次数")]
        public int LikeTimes { get; set; }
        [Display(Name = "回复次数")]
        public int ReplyTimes { get; set; }
        [Display(Name = "最新回复")]
        public int? LatestReplyID { get; set; }
       
        public virtual ApplicationUser User { get; set; }
        
         public Comment()
        {
           // this.ReadTimes = 0;
            this.LikeTimes = 0;
            this.CreateTime = DateTime.Now;
        }

         public void AddLike()
        {
            this.LikeTimes = this.LikeTimes + 1;
        }

        public void CancelLike()
        {
            this.LikeTimes = this.LikeTimes - 1;
        }

        //public IOrderedQueryable<Reply>  GetReply(){
        //    var replys=from r in db.Replys
        //              where r.ReplyType==ReplyType.Comment &&r.ReplyID==this.CommentID
        //              orderby r.CreateTime
        //              select r;
        //   return replys;            
        //}

        //public IEnumerable<Reply> GetReply()
        //{
        //    var query = from r in db.Replys
        //                where r.ReplyType == ReplyType.Comment && r.ReplyID == this.CommentID
        //                select r;

        //    return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
        //}


        //private IEnumerable<Reply> GetSonID(int p_id)
        //{
        //    var query = from c in db.Replys
        //                where c.ReplyID == p_id && c.ReplyType == ReplyType.Reply
        //                select c;

        //    return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
        //}  
    }
}