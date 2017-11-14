using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Holder
    {
        public int ID { get; set; }

        /// <summary>
        /// 分红权拥有人
        /// </summary>
        [Display(Name = "分红权拥有人")]
        public string UserID { get; set; }

        /// <summary>
        /// 分红权股份
        /// </summary>
        [Display(Name = "分红权股份")]
        public int Stock { get; set; }
        
        /// <summary>
        /// 身份证号码
        /// </summary>
        [Display(Name = "身份证号码")]
        public string IDCard { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        public string BankName { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        [Display(Name = "电话号码")]
        public string Phone { get; set; }

    }
}