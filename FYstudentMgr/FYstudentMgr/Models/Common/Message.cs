using studentManager.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace studentManager.Models.Common
{
    public class Message : ReplayObject
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int MessageID { get; set; }
        public int UserID { get; set; }
        public int ToUserID { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LeaveDate { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("ToUserID")]
        public virtual ApplicationUser ToUser { get; set; }
        public Boolean IsCHeck { get; set; }
        public Boolean IsRead { get; set; }
        public Message() {
            this.LeaveDate = DateTime.Now;
        }

        public int GetID()
        {
            return this.MessageID;
        }
        public string GetTitle() {
            return this.Content;
        }

        public string GetClassID()
        {
            return "";
        }
        public IEnumerable<Reply> GetReplys()
        {
            var replays = db.Replys.Where(r => r.ReplyID == this.MessageID)
                        .Where(r => r.ReplyType == ReplyType.Message)
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
                        where c.ReplyID == p_id && c.Level>1
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonID(t.ID)));
        }
        public int GetSubjectID()
        {
            return 0;
        }
    }
}