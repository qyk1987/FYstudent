using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public enum LikeType{
        Comment,
        Discuss,
        Article,
        Reply,
        Material,
        Note
    }
    public class Like
    {
        public int LikeID { get; set; }
        public int UserID { get; set; }
        public int ToID { get; set; }
        public Boolean IsRead { get; set; }
        public LikeType LikeType {get;set;}
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LikeDate { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Like()
        {
            this.LikeDate = DateTime.Now;
        }
    }
    
}