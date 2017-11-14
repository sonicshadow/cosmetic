using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Product
    {
        public int ID { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [Display(Name = "商品名称")]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 零售价格
        /// </summary>
        [Display(Name = "零售价格")]
        public decimal Price { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Display(Name = "商品描述")]
        [Required]
        public string Info { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "规格")]
        public string Spec { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        [Display(Name = "条码")]
        [Required]
        public string Number { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        [Display(Name = "货号")]
        public string No { get; set; }

        /// <summary>
        /// 计量单位
        /// </summary>
        [Display(Name = "计量单位")]
        [Required]
        public string Unit { get; set; }

        /// <summary>
        /// 上市时间
        /// </summary>
        [Display(Name = "上市时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Release { get; set; }

        public virtual List<SupplierProduct> SupplierProducts { get; set; }

        /// <summary>
        /// 销售单价
        /// </summary>
        [Display(Name = "销售单价")]
        public virtual List<ProductDetail> ProductDetail { get; set; }
    }

    [NotMapped]
    public class ProductViewModel:Product
    {
        public ProductViewModel()
        {
        }

        public ProductViewModel(Product product, UserProduct userProduct)
        {
            ID = product.ID;
            Info = product.Info;
            Name=product.Name;
            Number= product.Number;
            Price= product.Price;
            Release= product.Release;
            Spec= product.Spec;
            Unit= product.Unit;
            Min = userProduct.TwiceMin;
            UserPrice = userProduct.Price;
        }
        
        /// <summary>
        /// 最少进货数量
        /// </summary>
        [Display(Name = "最少进货数量")]
        public int Min { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "单价")]
        public decimal UserPrice { get; set; }

    }
}