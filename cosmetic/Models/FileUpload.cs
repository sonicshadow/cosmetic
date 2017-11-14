﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class FileUpload
    {
        /// <summary>
        /// 控件名称（不能重复不能为空）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 已有图片链接
        /// </summary>
        public string[] Images { get; set; } = new string[0];

        /// <summary>
        //  最多可上传
        /// </summary>
        public int Max { get; set; } = 1;

        public bool AutoInit { get; set; } = true;

        public string Value
        {
            get
            {
                return JsonConvert.SerializeObject(Images);
            }
            set
            {
                Images = JsonConvert.DeserializeObject<string[]>(value);
            }
        }

        /// <summary>
        /// 可以排序
        /// </summary>
        public bool Sortable { get; set; }


        /// <summary>
        /// 取消重命名
        /// </summary>
        [Display(Name = "取消重命名")]
        public bool IsResetName { get; set; }

        /// <summary>
        /// 路径
        ///  <para>为空表示使用默认路径</para>
        /// </summary>
        [Display(Name = "路径")]
        public string FilePath { get; set; }

        /// <summary>
        /// 上传模式
        /// </summary>
        [Display(Name = "上传模式")]
        public FileType Type { get; set; }

        [Display(Name = "上传者ID")]
        public string UserID { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public FileUploadMode Mode { get; set; }
    }

    public enum FileType
    {
        /// <summary>
        /// 所有类型
        /// </summary>
        [Display(Name = "所有类型")]
        File,
        /// <summary>
        /// 图片
        /// </summary>
        [Display(Name = "图片")]
        Image,
        /// <summary>
        /// 文档
        /// </summary>
        [Display(Name = "文档")]
        Word,
        /// <summary>
        /// Excel
        /// </summary>
        [Display(Name = "Excel")]
        Excel,
        /// <summary>
        /// 视频
        /// </summary>
        [Display(Name = "视频")]
        Video,
        /// <summary>
        /// 声音
        /// </summary>
        [Display(Name = "声音")]
        Sound,
    }

    public enum FileUploadMode
    {
        /// <summary>
        /// 默认模式
        /// </summary>
        Default,
        /// <summary>
        /// 只有弹窗
        /// </summary>
        ModalOnly
    }
}