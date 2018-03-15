using FYstudentMgr.Common;
using FYstudentMgr.Models.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public enum ReplyType{
        Article,     
       Discuss,
        Message,      
       Question
        
    }
    public class Reply
    {
        private string content;
        private ApplicationDbContext db = new ApplicationDbContext();
        [Display(Name = "回复编号")]
        public int ID { get; set; }
        [Display(Name = "回复对象编号")]
        public int ReplyID { get; set; }

        [Display(Name = "根对象编号")]
        public int RootID { get; set; }
        [Display(Name = "回复的级别")]
        public int Level { get; set; }
        [Display(Name = "回复对象类型")]
        public ReplyType ReplyType { get; set; }

        public int? ActiveLogID { get; set; }
        [Required]
        [Display(Name = "回复内容")]
        [DataType(DataType.MultilineText)]
        public string Content
        {
            get { return content; }
            set
            {
                Regex r1 = new Regex(@"^<p>[\w|\W]*</p>$");
                if (r1.IsMatch(value))
                {
                    content = value;
                    content = content.Remove(0, 3);
                    content = content.Remove(content.Length - 4, 4);
                }
                else
                {
                    content = value;
                }

            }
        }
        [Display(Name = "作者")]
        public int UserID { get; set; }
        [Display(Name = "对方用户编号")]
        public int? ToUserID { get; set; }
        [Display(Name = "回复时间")]
        public DateTime CreateTime { get; set; }
        //public virtual Comment Comment { get; set; }
        [Display(Name = "被赞次数")]
        public int LikeTimes { get; set; }

        public Boolean IsRead { get; set; }
        public Boolean IsCHeck { get; set; }
        /// <summary>
        /// 添加赞
        /// </summary>
        public void AddLike()
        {
            this.LikeTimes = this.LikeTimes + 1;
        }

        /// <summary>
        /// 取消赞
        /// </summary>
        public void CancelLike()
        {
            this.LikeTimes = this.LikeTimes - 1;
        }
       
        public Reply() {
            this.LikeTimes = 0;
            this.CreateTime = DateTime.Now;
        }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("ToUserID")]
        public virtual ApplicationUser ToUser { get; set; }
        public virtual ActiveLog ActiveLog { get; set; }
        

        /// <summary>
        /// 返回该回复的最终回复对象
        /// </summary>
        /// <returns></returns>
        public ReplayObject GetObject()
        {
            if (this.ReplyType == ReplyType.Article)
            {
                Article article = db.Articles.Find(this.RootID);
                return article;
            }
            else if (this.ReplyType == ReplyType.Discuss)
            {
                Discuss discuss = db.Discusss.Find(this.RootID);
                return discuss;
            }
            else 
            
            {
                Question question = db.Questions.Find(this.RootID);
                return question;
            }
        }




        public IEnumerable<Reply> GetReplys()
        {
            var query = from c in db.Replys
                        where c.ReplyID == this.ID && c.Level>1
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
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
    }
}