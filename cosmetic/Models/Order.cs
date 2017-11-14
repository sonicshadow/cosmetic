using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order : IPayee
    {
        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        public string Code { get; set; }

        /// <summary>
        /// 合伙人
        /// </summary>
        [Display(Name = "合伙人")]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Display(Name = "产品")]
        public int ProductID { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "单价")]
        public decimal Price { get; set; }

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

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        public Enums.OrderState State { get; set; }

        /// <summary>
        /// 已发货数量
        /// </summary>
        [Display(Name = "已发货数量")]
        public int Send { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        [Display(Name = "收款时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceiptDateTime { get; set; }

        /// <summary>
        /// 发货记录
        /// </summary>
        [Display(Name = "发货记录")]
        public virtual List<DeliverHistory> DeliverHistory { get; set; }

        /// <summary>
        /// 是否已打款
        /// </summary>
        [Display(Name = "是否已打款")]
        public bool IsPay { get; set; }

        /// <summary>
        /// 订单发货人，null就是公司发货
        /// </summary>
        [Display(Name = "订单发货人")]
        public string ParentUser { get; set; }

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
        /// 银行卡
        /// </summary>
        [Display(Name = "银行卡")]
        public string BankCard { get; set; }


        /// <summary>
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人")]
        [NotMapped]
        public string Recommend { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [NotMapped]
        public Product Products { get; set; }
    }

    [NotMapped]
    public class OrderViewModel : Order
    {
        public OrderViewModel(Order order, Product product)
        {
            ID = order.ID;
            Code = order.Code;
            Count = order.Count;
            User = order.User;
            UserID = order.UserID;
            Send = order.Send;
            ProductID = order.ProductID;
            Product = product;
            ReceiptDateTime = order.ReceiptDateTime;
            CreateDateTime = order.CreateDateTime;
            DeliverHistory = order.DeliverHistory;
            Price = order.Price;
            State = order.State;
            Total = order.Total;
            IsPay = order.IsPay;
            BankAccount = order.BankAccount;
            BankCard = order.BankCard;
            ParentUser = order.ParentUser;
            BankName = order.BankName;
        }

        /// <summary>
        /// 产品
        /// </summary>
        [Display(Name = "产品")]
        public Product Product { get; set; }

    }
    
    public class OrderFoot
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        [Display(Name = "产品")]
        public int ProductID { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [Display(Name = "产品")]
        public Product Product { get; set; }

        public decimal Total { get; set; }

        public int Count { get; set; }

        public int Send { get; set; }

        public int Owe { get; set; }

        public bool IsStock { get; set; }
    }
}