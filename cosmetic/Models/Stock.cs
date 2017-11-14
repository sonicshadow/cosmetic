using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    /// <summary>
    /// 库存记录
    /// </summary>
    public class Stock
    {
        public int ID { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = "商品")]
        public int ProductID { get; set; }
        public Product Product { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        [Display(Name = "操作人")]
        public string UserID { get; set; }

        /// <summary>
        /// 记录类型
        /// </summary>
        [Display(Name = "记录类型")]
        public Enums.StockType Type { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Display(Name = "数量")]
        public int Count { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        public string Remark { get; set; }

        /// <summary>
        /// 如果UserID==null,DataID是库存id，则dataid是订单id
        /// </summary>
        [Display(Name = "数据ID")]
        public int DataID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }
    }

    [NotMapped]
    public class StockViewModel : Stock
    {
        public StockViewModel() { }

        public StockViewModel(Stock stock, ApplicationUser user, string supplier, string no)
        {
            ID = stock.ID;
            Product = stock.Product;
            ProductID = stock.ProductID;
            UserID = stock.UserID;
            Type = stock.Type;
            Count = stock.Count;
            Remark = stock.Remark;
            CreateTime = stock.CreateTime;
            NO = no;
            Remark = stock.Remark;
            if (stock.Type == Enums.StockType.AddStock || stock.Type == Enums.StockType.Deliver || stock.Type == Enums.StockType.Replacement)
            {
                Supplier = supplier;
            }
            if (user != null)
            {
                if (user.Type == Enums.User.Normal)
                {
                    UserName = user.UserName;
                    Rank = user.Rank;
                }
            }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        public string Supplier { get; set; }

        /// <summary>
        /// 进出单号
        /// </summary>
        [Display(Name = "进出单号")]
        public string NO { get; set; }

        /// <summary>
        /// 合伙人级别
        /// </summary>
        [Display(Name = "合伙人级别")]
        public Enums.UserType? Rank { get; set; }

        /// <summary>
        /// 会员号
        /// </summary>
        [Display(Name = "会员号")]
        public string UserName { get; set; }

    }

}