using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public enum Status
    {
        NotRead,
        Readed,
        Delete
    }
    public class Mail
    {
        public int MailID { get; set; }
        public int RecID { get; set; }
        public int MailTextID { get; set; }

        public Status Status { get; set; }


        [ForeignKey("RecID")]
        public virtual ApplicationUser Reciever { get; set; }
        public virtual MailText MailText { get; set; }
    }
}