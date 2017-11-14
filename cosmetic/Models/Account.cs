using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    /// <summary>
    /// 账目记录
    /// </summary>
    public class Account :IPayee
    {
        public int ID { get; set; }

        /// <summary>
        /// 类目
        /// </summary>
        [Display(Name = "类目")]
        public int AccountKindID { get; set; }        
        public virtual AccountKind AccountKind { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime UpdateDateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [DataType(DataType.MultilineText)]
        public string Remark { get; set; }
        
        /// <summary>
        /// 银行账户
        /// </summary>
        [Display(Name = "银行账户")]
        public string BankAccount { get; set; }

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
        /// 手续费
        /// </summary>
        [Display(Name = "手续费")]
        public decimal Fee { get; set; }

        /// <summary>
        /// 交易人
        /// </summary>
        [Display(Name = "交易人")]
        public string Trader { get; set; }

        /// <summary>
        /// 交易人类型
        /// </summary>
        [Display(Name = "交易人类型")]
        public Enums.TraderType TraderType { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        public Enums.UserType? UserType { get; set; }

        /// <summary>
        /// 已取消
        /// </summary>
        [Display(Name = "已取消")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// 可取消
        /// </summary>
        [Display(Name = "可取消")]
        public bool AllowDelete { get; set; }

        /// <summary>
        /// 结存
        /// </summary>
        [NotMapped]
        [Display(Name = "结存")]
        public decimal Totla { get; set; }

    }

    /// <summary>
    /// 账目类别
    /// </summary>
    public class AccountKind
    {
        public int ID { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [Display(Name = "分类")]
        public Enums.AccountKindType Type { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        [Display(Name = "名字")]
        public string Name { get; set; }

        public virtual List<Account> Accounts { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        [Display(Name = "编号")]
        public string Code { get; set; }


    }
}