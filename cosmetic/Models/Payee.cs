using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Payee
    {
        public Payee() { }

        public int ID { get; set; }

        /// <summary>
        /// 帐户户名
        /// </summary>
        [Display(Name = "帐户户名")]
        public string Name { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        public string Bank { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { get; set; }

        /// <summary>
        /// 帐户结存金额
        /// </summary>
        [Display(Name = "帐户结存金额")]
        [NotMapped]
        public decimal Amount { get; set; }

    }
}