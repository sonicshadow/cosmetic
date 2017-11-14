using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department
    {

        public int ID { get; set; }

        /// <summary>
        /// 部门编号
        /// </summary>
        [Display(Name = "部门编号")]
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "部门名称")]
        public string Name { get; set; }

        /// <summary>
        /// 员工
        /// </summary>
        [Display(Name = "员工")]
        public virtual List<Staff> Staffs { get; set; }
    }
}