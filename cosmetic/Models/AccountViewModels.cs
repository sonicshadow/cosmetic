using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "电子邮件")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        public RegisterViewModel() { }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Phone]
        [Display(Name = "电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        ///// <summary>
        ///// 身份证号码
        ///// </summary>
        //[Required]
        //[Display(Name = "身份证号码")]
        //public string IDCard { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [Display(Name = "开户行名称")]
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
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人手机号")]
        [Required]
        public string Recommend { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        public Enums.UserType Type { get; set; }

        /// <summary>
        /// 网点联行号
        /// </summary>
        [Display(Name = "网点联行号")]
        public string BankCode { get; set; }

        public string IDCard { get; set; }

        public DateTime Time { get; set; }
    }

    public class RegisterByUserViewModel
    {
        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [Phone]
        [RegularExpression(Reg.MOBILE)]
        [Display(Name = "电话")]
        public string Phone { get; set; }


        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        ///// <summary>
        ///// 身份证号码
        ///// </summary>
        //[Required]
        //[Display(Name = "身份证号码")]
        //[RegularExpression(Reg.IDCARD)]
        //public string IDCard { get; set; }

        /// <summary>
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人")]
        public string Recommend { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [Display(Name = "等级")]
        public Enums.UserType Rank { get; set; }

    }

    public class UserSettingModel
    {

        [Display(Name = "订单")]
        public Order Order { get; set; }

        [Display(Name = "用户商品")]
        public List<UserProduct> UserProduct { get; set; }
    }

    public class RegisterAdmin
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Display(Name = "电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 身份证号码
        /// </summary>
        [Required]
        [Display(Name = "身份证号码")]
        [RegularExpression(Reg.IDCARD)]
        public string IDCard { get; set; }

        /// <summary>
        /// 开户行名称
        /// </summary>
        [Required]
        [Display(Name = "开户行名称")]
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
        /// 角色
        /// </summary>
        [Display(Name = "角色")]
        public int RoleGroupID { get; set; }

        /// <summary>
        /// 网点联行号
        /// </summary>
        [Display(Name = "网点联行号")]
        public string BankCode { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("NewPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "手机号码")]
        public string Phone { get; set; }
    }
}
