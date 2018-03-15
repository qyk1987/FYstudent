using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Configurations
{
    public class MailApiConfig : ConfigurationSection
    {

        /// <summary>
        /// 发信地址
        /// </summary>
        [ConfigurationProperty("accountName",IsRequired = true)]
        public string AccountName
        {
            get
            {
                return (string)this["accountName"];
            }
            set
            {
                this["accountName"] = value;
            }
        }
        /// <summary>
        /// 发信人昵称
        /// </summary>
        [ConfigurationProperty("fromAlias", IsRequired = true)]
        public string FromAlias
        {
            get
            {
                return (string)this["fromAlias"];
            }
            set
            {
                this["fromAlias"] = value;
            }
        }
        /// <summary>
        /// 默认端口25（设为-1让系统自动设置）
        /// </summary>
        [ConfigurationProperty("addressType", DefaultValue = "1", IsRequired = true)]
        public int AddressType
        {
            get
            {
                return (int)this["addressType"];
            }
            set
            {
                this["addressType"] = value;
            }
        }
       
      
        /// <summary>
        /// 是否回复
        /// </summary>
        [ConfigurationProperty("replyToAddress", DefaultValue = "true", IsRequired = true)]
        public bool ReplyToAddress
        {
            get
            {
                return (bool)this["replyToAddress"];
            }
            set
            {
                this["replyToAddress"] = value;
            }
        }
       

    }
}