using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class DeliverHistory
    {
        public int ID { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        [Display(Name = "发货数量")]
        public int Count { get; set; }

        /// <summary>
        /// 会员
        /// </summary>
        [Display(Name = "会员")]
        public string UserID { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 快递名
        /// </summary>
        [Display(Name = "快递名")]
        public string Express { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        [Display(Name = "快递单号")]
        public string Code { get; set; }

        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckTime { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public Enums.CheckState CheckState { get; set; }

        /// <summary>
        /// 审核人员
        /// </summary>
        [Display(Name = "审核人员")]
        public string CheckUser { get; set; }

        /// <summary>
        /// 发货的供应商
        /// </summary>
        [Display(Name = "发货的供应商")]
        public int DataID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Display(Name = "订单")]
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }

    }
}