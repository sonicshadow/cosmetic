using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Display
    {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public int ID { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 副标题
        /// </summary>
        [Display(Name = "副标题")]
        public string Subtitle { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [Display(Name = "标签")]
        public string Tag { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        public string Images { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        [Display(Name = "规格")]
        public string Specification { get; set; }

        /// <summary>
        /// 商品介绍
        /// </summary>
        [Display(Name = "商品介绍")]
        public string Content { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [Display(Name = "价格")]
        public decimal Price { get; set; }
    }

    [NotMapped]
    public class DisplayViewModel :Display
    {
        public DisplayViewModel() { }

        public DisplayViewModel(Display d)
        {
            ID = d.ID;
            Title = d.Title;
            Tag = d.Tag;
            Images = d.Images;
            Subtitle = d.Subtitle;
            Content = d.Content;
            Specification = d.Specification;
            Price = d.Price;
            FileUpload = new FileUpload() {
                Images = d.Images.SplitToArray<string>().ToArray(),
                AutoInit = true,
                Mode = FileUploadMode.Default,
                Type = FileType.Image,
                Name = "FileUpload",
                Max = 9
            };
        }

        public FileUpload FileUpload { get; set; }
    }
}