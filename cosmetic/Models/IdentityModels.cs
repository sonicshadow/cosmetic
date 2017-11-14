using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models
{
    // 可以通过向 ApplicationUser 类添加更多属性来为用户添加配置文件数据。若要了解详细信息，请访问 http://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [Display(Name = "真实姓名")]
        public string RealName { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Display(Name = "用户类型")]
        public Enums.User Type { get; set; }

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
        /// 地址
        /// </summary>
        [Display(Name = "地址")]
        public string Address { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        [Display(Name = "分组")]
        public int RoleGroupID { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        [Display(Name = "分组")]
        [NotMapped]
        public string RoleGroup { get; set; }

        /// <summary>
        /// 合伙人级别
        /// </summary>
        [Display(Name = "合伙人级别")]
        public Enums.UserType Rank { get; set; }

        /// <summary>
        /// 上级发货人ID，直接向公司拿货为null
        /// </summary>
        [Display(Name = "上级发货人ID")]
        public string Parent { get; set; }

        /// <summary>
        /// 所有上级发货人[Parent][Parent]...存储
        /// </summary>
        [Display(Name = "所有上级发货人")]
        public string AllParent { get; set; }

        /// <summary>
        /// 推荐人ID,推荐人是公司为null
        /// </summary>
        [Display(Name = "推荐人")]
        public string Recommend { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        [Display(Name = "用户状态")]
        public Enums.UserState State { get; set; }
        
        /// <summary>
        /// 网点联行号
        /// </summary>
        [Display(Name = "网点联行号")]
        public string BankCode { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Display(Name = "注册时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime RegisterDateTime { get; set; }

        ///// <summary>
        ///// 首次下单产品
        ///// </summary>
        //[Display(Name = "首次下单产品")]
        //public int FirstProduct { get; set; }

        [Display(Name = "最后一次登录日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastLoginDateTime { get; set; }

        public virtual List<UserIncome> UserIncomes { get; set; }

    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }

        public string Group { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public virtual string Description { get; set; }
    }


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<RoleGroup> RoleGroups { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<SupplierProduct> SupplierProducts { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }

        public DbSet<UserProduct> UserProducts { get; set; }

        public DbSet<DeliverHistory> DeliverHistories { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountKind> AccountKinds { get; set; }

        public DbSet<UserIncome> UserIncomes { get; set; }

        public DbSet<Mission> Missions { get; set; }

        public DbSet<MissionDetail> MissionDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<Payee> Payees { get; set; }

        public DbSet<SystemSetting> SystemSettings { get; set; }
        
        /// <summary>
        /// 公告
        /// </summary>
        public DbSet<Notice> Notices { get; set; }

        /// <summary>
        /// 橱窗
        /// </summary>
        public DbSet<Display> Displays { get; set; }

        /// <summary>
        /// 股东股份
        /// </summary>
        public DbSet<Holder> Holders { get; set; }

        /// <summary>
        /// 修改信息
        /// </summary>
        public DbSet<Modify> Modifys { get; set; }

        /// <summary>
        /// 退换货
        /// </summary>
        public DbSet<Return> Returns { get; set; }

    }
}