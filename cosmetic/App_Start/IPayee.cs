using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic
{
    interface IPayee
    {
        /// <summary>
        /// 银行账户
        /// </summary>
        [Display(Name = "银行账户")]
        string BankAccount { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        string BankName { get; set; }

        /// <summary>
        /// 银行卡
        /// </summary>
        [Display(Name = "银行卡")]
        string BankCard { get; set; }
    }
}