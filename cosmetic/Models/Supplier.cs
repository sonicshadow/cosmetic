using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Models
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class Supplier
    {
        public int ID { get; set; }

        /// <summary>
        /// 供应商名称
        /// </summary>
        [Display(Name = "供应商名称")]
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        [Display(Name = "联系人")]
        public string Contacts { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        public string PhoneNumber { get; set; }

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
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = "商品")]
        public virtual List<SupplierProduct> SupplierProducts { get; set; }


    }

}