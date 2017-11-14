using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class Staff
    {
        public int ID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 职责
        /// </summary>
        [Display(Name = "职责")]
        public string Work { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [Display(Name = "身份证")]
        public string IDCard { get; set; }

        /// <summary>
        /// 家庭住址
        /// </summary>
        [Display(Name = "家庭住址")]
        public string Address { get; set; }

        /// <summary>
        /// 基本薪资
        /// </summary>
        [Display(Name = "基本薪资")]
        public decimal BasicSalary { get; set; }

        /// <summary>
        /// 应加薪资
        /// </summary>
        [Display(Name = "应加薪资")]
        public decimal PlusSalary { get; set; }

        /// <summary>
        /// 扣除薪资
        /// </summary>
        [Display(Name = "扣除薪资")]
        public decimal DeductSalary { get; set; }

        /// <summary>
        /// 入职日期
        /// </summary>
        [Display(Name = "入职日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EntryTime { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Display(Name = "部门")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
    }
}