using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Modify
    {
        public int ID { get; set; }

        /// <summary>
        /// 新数据
        /// </summary>
        [Display(Name ="新数据")]
        public string NewData { get; set; }

        /// <summary>
        /// 旧数据
        /// </summary>
        [Display(Name = "旧数据")]
        public string OldData { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        public Enums.ModifyType ModifyType { get; set; }

    }

    public class AccountModify
    {
        /// <summary>
        /// 资金记录
        /// </summary>
        [Display(Name = "资金记录")]
        public Account Account { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        /// <summary>
        /// 取消原因
        /// </summary>
        [Display(Name = "取消原因")]
        public string Remark { get; set; }
    }
}