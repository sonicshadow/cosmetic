using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models
{
    /// <summary>
    /// 用户商品
    /// </summary>
    public class UserProduct
    {

        public UserProduct() { }

        public int ID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        [Display(Name = "商品")]
        public int ProductID { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// 首批最低进货数量
        /// </summary>
        [Display(Name = "首批最低进货数量")]
        public int Min { get; set; }

        /// <summary>
        /// 二次最低进货数量
        /// </summary>
        [Display(Name = "二次最低进货数量")]
        public int TwiceMin { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "单价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 已发货（已经发出求的数量）
        /// </summary>
        [Display(Name = "已发货")]
        public int Sent { get; set; }

        /// <summary>
        /// 总量(一共收到的数量)
        /// </summary>
        [Display(Name = "总量")]
        public int Sum { get; set; }

        /// <summary>
        /// 库存（剩余手中的数量）
        /// </summary>
        [Display(Name = "库存")]
        public int Count { get; set; }

    }

    /// <summary>
    /// 下级的信息
    /// </summary>
    public class UserProductViewModel
    {
        public UserProductViewModel() { }

        public UserProductViewModel(UserProduct userProduct, ApplicationUser parent, ApplicationUser recommend)
        {
            ID = userProduct.ID;
            UserID = userProduct.UserID;
            ProductName = userProduct.Product.Name;
            ProductID = userProduct.ProductID;
            Rank = userProduct.User.Rank;
            Count = userProduct.Count;
            PhoneNumber = userProduct.User.PhoneNumber;
            RealName = userProduct.User.RealName;
            ParentID = parent==null?"": parent.Id;
            ParentName = parent == null ? "公司" : parent.RealName;
            RecommendID = recommend == null ? "" : recommend.Id;
            RecommendName = recommend == null ? "公司" : recommend.RealName;
            RegisterDateTime = userProduct.User.RegisterDateTime;
        }

        public int ID { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string UserID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [Display(Name = "等级")]
        public Enums.UserType Rank { get; set; }

        /// <summary>
        /// 上级发货人ID，直接向公司拿货为null
        /// </summary>
        [Display(Name = "上级发货人")]
        public string ParentID { get; set; }

        /// <summary>
        /// 上级发货人姓名，直接向公司拿货为null
        /// </summary>
        [Display(Name = "上级发货人")]
        public string ParentName { get; set; }

        /// <summary>
        /// 上级发货人ID,推荐人是公司为null
        /// </summary>
        [Display(Name = "推荐人")]
        public string RecommendID { get; set; }


        /// <summary>
        /// 上级发货人姓名，直接向公司拿货为null
        /// </summary>
        [Display(Name = "推荐人")]
        public string RecommendName { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        [Display(Name = "商品ID")]
        public int ProductID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Display(Name = "库存")]
        public int Count { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime RegisterDateTime { get; set; }
    }
}