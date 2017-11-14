using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class ProductDetail
    {
        public int ID { get; set; }

        [Display(Name = "级别")]
        public Enums.UserType UseType { get; set; }

        [Display(Name = "首批最低进货数量")]
        public int Min { get; set; }

        [Display(Name = "二次最低进货数量")]
        public int TwiceMin { get; set; }

        [Required]
        [Display(Name = "单价")]
        public decimal Price { get; set; }

        public int ProductID { get; set; }

        public virtual Product Product { get; set; }

    }
}