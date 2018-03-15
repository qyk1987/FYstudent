using FYstudentMgr.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Models.Common
{
    public enum MailType{
      Private,
      Public,
      Global
    }
    public class MailText
    {
        public int MailTextID { get; set; }
        public int SendID { get; set; }
        public string Content { get; set; }
        public MailType Type { get; set; }
        public int? GroupID { get; set; }
        public DateTime PostDate { get; set; }

        [ForeignKey("SendID")]
        public virtual ApplicationUser Sender { get; set; }
        public virtual ICollection<Mail> Mails { get; set; }
    }
}