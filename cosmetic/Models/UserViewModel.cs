using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models
{
    public class UserViewModel
    {
        public string ID { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Display(Name = "身份证号码")]
        public string IDCard { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        public string Bank { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { get; set; }

        /// <summary>
        /// 网点联行号
        /// </summary>
        [Display(Name = "网点联行号")]
        public string BankCode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public int RoleGroupID { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        public Enums.User Type { get; set; }

        /// <summary>
        /// 合伙人级别
        /// </summary>
        [Display(Name = "合伙人级别")]
        public Enums.UserType Rank { get; set; }

        /// <summary>
        /// 上级发货人ID，直接向公司拿货为null
        /// </summary>
        [Display(Name = "上级发货人")]
        public string Parent { get; set; }

        /// <summary>
        /// 上级发货人姓名，直接向公司拿货为null
        /// </summary>
        [Display(Name = "上级发货人")]
        public string ParentName { get; set; }

        /// <summary>
        /// 推荐人ID,推荐人是公司为null
        /// </summary>
        [Display(Name = "推荐人手机号")]
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐人姓名,推荐人是公司为null
        /// </summary>
        [Display(Name = "推荐人")]
        public string RecommendName { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        [Display(Name = "库存")]
        public int Count { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        [Display(Name = "产品")]
        public List<UserProduct> UserProduct { get; set; }

    }

    public class TeamModel
    {
        public TeamModel()
        {
        }

        public TeamModel(ApplicationUser user)
        {
            User = user;
            Child = new List<TeamModel>();
        }

        public ApplicationUser User { get; set; }

        public List<TeamModel> Child { set; get; } = new List<TeamModel>();
        
    }
    
    public class UserList
    {
        public UserList() { }
        public string ID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [Display(Name = "等级")]
        public Enums.UserType Rank { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人")]
        public string Recommend { get; set; }

        /// <summary>
        /// 订单发货人
        /// </summary>
        [Display(Name = "订单发货人")]
        public string Parent { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        public string IDCard { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        public string Bank { get; set; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }
        
        /// <summary>
        /// 用户状态
        /// </summary>
        [Display(Name = "用户状态")]
        public Enums.UserState UserState { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDateTime { get; set; }
    }

}