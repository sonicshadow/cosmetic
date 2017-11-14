using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosmetic.Models
{
    public class Notice
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name ="ID")]
        public int ID { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Display(Name = "内容")]
        public string Content { get; set; }
        
        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        public int Sort { get; set; }

        /// <summary>
        /// 发布人
        /// </summary>
        [Display(Name = "发布人")]
        public string Publisher { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Display(Name = "副标题")]
        public string Subtitle { get; set; }
    }
}