using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class SupplierProduct 
    {
        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Display(Name = "订单编号")]
        public string Code { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = "商品")]
        public int ProductID { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = "商品")]
        public virtual Product Product { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        public int SupplierID { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        [Display(Name = "货号")]
        public string Number { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [Display(Name = "订单数量")]
        public int Count { get; set; }

        /// <summary>
        /// 已收数量
        /// </summary>
        [Display(Name = "已收数量")]
        public int Send { get; set; }

        /// <summary>
        /// 可发数量
        /// </summary>
        [Display(Name = "可发数量")]
        public int Remaining { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        [Display(Name = "采购单价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 订单总额
        /// </summary>
        [Display(Name = "订单总额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 已付金额
        /// </summary>
        [Display(Name = "已付金额")]
        public decimal SendTotal { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        public Enums.PurchaseOrderState State { get; set; }
    }

    [NotMapped]
    public class SupplierProductViewModel :SupplierProduct
    {
        public SupplierProductViewModel(SupplierProduct sp)
        {
            ID = sp.ID;
            Code = sp.Code;
            Count = sp.Count;
            CreateTime = sp.CreateTime;
            Number = sp.Number;
            ProductID = sp.ProductID;
            Product = sp.Product;
            Price = sp.Price;
            Supplier = sp.Supplier;
            Remaining = sp.Remaining;
            Send = sp.Send;
            SupplierID = sp.SupplierID;
            SendTotal = sp.SendTotal;
            State = sp.State;
            IsSendCount = sp.Count - sp.Send;
            Total = sp.Total;
            IsSendTotal = sp.Total - sp.SendTotal;
            SendOutCount = sp.Send - sp.Remaining;
        }

        public SupplierProductViewModel() { }

        /// <summary>
        /// 未收货物
        /// </summary>
        [Display(Name = "未收货物")]
        public int IsSendCount { get; set; }

        /// <summary>
        /// 未付款金额
        /// </summary>
        [Display(Name ="未付款金额")]
        public decimal IsSendTotal { get; set; }

        /// <summary>
        /// 发出货物
        /// </summary>
        [Display(Name = "发出货物")]
        public int SendOutCount { get; set; }
    }
}