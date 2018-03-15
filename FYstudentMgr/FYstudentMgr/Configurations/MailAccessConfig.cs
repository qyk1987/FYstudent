using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FYstudentMgr.Configurations
{
    public class MailAccessConfig : ConfigurationSection
    {
        /// <summary>
        /// 发信地址
        /// </summary>
        [ConfigurationProperty("accessKey", IsRequired = true)]
        public string AccessKey
        {
            get
            {
                return (string)this["accessKey"];
            }
            set
            {
                this["accessKey"] = value;
            }
        }
        /// <summary>
        /// 发信人昵称
        /// </summary>
        [ConfigurationProperty("accessSecret", IsRequired = true)]
        public string AccessSecret
        {
            get
            {
                return (string)this["accessSecret"];
            }
            set
            {
                this["accessSecret"] = value;
            }
        }
    }
}