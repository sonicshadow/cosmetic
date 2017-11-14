using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Return
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [Display(Name = "申请人")]
        public string User { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        [Display(Name = "申请时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 旧订单号
        /// </summary>
        [Display(Name = "旧订单号")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        /// <summary>
        /// 新商品
        /// </summary>
        [Display(Name = "新商品")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Count { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "单价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public Enums.CheckState CheckState { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Display(Name = "审核人")]
        public string CheckUser { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        [Display(Name = "收款时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        [Display(Name = "收货时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceiptTime { get; set; }

    }


    public class ReturnViewModel
    {
        public ReturnViewModel() { }

        public ReturnViewModel(Return r)
        {
            ID = r.ID;
            User = r.User;
            Time = r.Time;
            OldProduct = r.Order.Products;
            Order = r.Order;
            NewProductID = r.ProductID;
            NewProduct = r.Product;
            Count = r.Count;
            Price = r.Price;
            Total = r.Total;
            Difference = r.Total - r.Order.Total;
            CheckState = r.CheckState;
            CheckTime = r.CheckTime;
            CheckUser = r.CheckUser;
            PayTime = r.PayTime;
            ReceiptTime = r.ReceiptTime;
        }

        public int ID { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [Display(Name = "申请人")]
        public string User { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        [Display(Name = "申请时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 原商品
        /// </summary>
        [Display(Name = "原商品")]
        public Product OldProduct { get; set; }
        
        /// <summary>
        /// 旧订单
        /// </summary>
        [Display(Name = "旧订单")]
        public Order Order { get; set; }

        /// <summary>
        /// 新商品
        /// </summary>
        [Display(Name = "新商品")]
        public int NewProductID { get; set; }

        /// <summary>
        /// 新商品
        /// </summary>
        [Display(Name = "新商品")]
        public Product NewProduct { get; set; }

        /// <summary>
        /// 对换后数量
        /// </summary>
        [Display(Name = "对换后数量")]
        public int Count { get; set; }

        /// <summary>
        /// 新商品单价
        /// </summary>
        [Display(Name = "新商品单价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 对换后总金额
        /// </summary>
        [Display(Name = "对换后总金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 补差价
        /// </summary>
        [Display(Name = "补差价")]
        public decimal Difference { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [Display(Name = "审核状态")]
        public Enums.CheckState CheckState { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>
        [Display(Name = "审核时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CheckTime { get; set; }

        /// <summary>
        /// 审核人
        /// </summary>
        [Display(Name = "审核人")]
        public string CheckUser { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        [Display(Name = "收款时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? PayTime { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        [Display(Name = "收货时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceiptTime { get; set; }
    }

    
}