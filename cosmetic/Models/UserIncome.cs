using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class UserIncome :IPayee
    {
        public int ID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }

        /// <summary>
        /// 被推荐人
        /// </summary>
        [Display(Name = "被推荐人")]
        public string RecommendID { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        public Enums.UserIncomeType Type { get; set; }

        /// <summary>
        /// 是否已打款
        /// </summary>
        [Display(Name = "是否已打款")]
        public bool IsPay { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 数据ID
        /// </summary>
        [Display(Name = "数据ID")]
        public int DateID { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        [Display(Name = "收款时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceiptDateTime { get; set; }
        
        /// <summary>
        /// 打款银行账户
        /// </summary>
        [Display(Name = "打款银行账户")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 打款银行名称
        /// </summary>
        [Display(Name = "打款银行名称")]
        public string BankName { get; set; }

        /// <summary>
        /// 打款银行卡
        /// </summary>
        [Display(Name = "打款银行卡")]
        public string BankCard { get; set; }
    }

    
    [NotMapped]
    public class UserIncomeViewModel :UserIncome
    {
        public UserIncomeViewModel() { }
        public UserIncomeViewModel(UserIncome ui,Product p,ApplicationUser au,Order o)
        {
            CreateDateTime = ui.CreateDateTime;
            RecommendID = ui.RecommendID;
            Recommend = au.RealName;
            RecommendRank = au.Rank;
            ProductName = p.Name;
            Count = o.Count;
            Total = o.Total;
            Amount = ui.Amount;
            IsPay = ui.IsPay;
            ReceiptDateTime = ui.ReceiptDateTime;
            BankName = ui.BankName;
            BankCard = ui.BankCard;
        }

        /// <summary>
        /// 被推荐人
        /// </summary>
        [Display(Name = "被推荐人")]
        public string Recommend { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        public Enums.UserType RecommendRank { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Count { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
        public decimal Total { get; set; }
    }
}