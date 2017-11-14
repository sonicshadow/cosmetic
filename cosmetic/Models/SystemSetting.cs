using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosmetic.Models
{
    public class SystemSetting
    {
        public int ID { get; set; }

        public Enums.SystemSettingType Key { get; set; }
        
        public string Value { get; set; }
    }

    public class MissionCompile
    {
        /// <summary>
        /// 内容
        /// </summary>
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "内容")]
        public string Value { get; set; }
    }
   
}