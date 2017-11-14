﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class RoleGroup
    {
        public int ID { get; set; }
        
        [Display(Name = "名称")]
        public string Name { get; set; }

        [Display(Name = "角色")]
        public string Roles { get; set; }
    }

    /// <summary>
    /// 组管理
    /// </summary>
    public class RoleGroupViewModel
    {
        public RoleGroupViewModel()
        {
            RolesList = new SelectRoleListViewModel();
        }

        public int ID { get; set; }

        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "未选择角色")]
        [Display(Name = "角色")]
        public string SelectedRoles { get; set; }

        /// <summary>
        /// 全部组列表
        /// </summary>
        public SelectRoleListViewModel RolesList { get; set; }
    }

    /// <summary>
    /// 全部组列表
    /// </summary>
    public class SelectRoleListViewModel
    {
        public SelectRoleListViewModel() { List = new List<SelectRoleView>(); }

        /// <summary>
        /// 角色列表
        /// </summary>
        public List<SelectRoleView> List
        {
            get; set;
        }
    }
    /// <summary>
    /// 角色列表
    /// </summary>
    public class SelectRoleView
    {
        /// <summary>
        /// 是否选择角色
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }

        /// <summary>
        /// 组名
        /// </summary>
        public string Group { get; set; }
    }
}