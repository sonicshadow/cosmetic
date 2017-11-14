using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cosmetic.Models
{
    public class NowStockViewModel
    {
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        [Display(Name = "库存数量")]
        public int Count { get; set; }

        [Display(Name = "所有者")]
        public string OwnerName { get; set; }

        [Display(Name = "单价")]
        public decimal Price { get; set; }

        [Display(Name = "合伙人级别")]
        public Enums.UserType? Rank { get; set; }

        [Display(Name = "推荐人")]
        public string Recommend { get; set; }

        [Display(Name = "会员号")]
        public string UserName { get; set; }

        [Display(Name = "身份证")]
        public string IDCard { get; set; }
    }


    public class SaleViewModel
    {
        public SaleViewModel()
        {
        }

        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        [Display(Name = "销售者")]
        public string Seller { get; set; }

        [Display(Name = "销售数量")]
        public int Count { get; set; }

        [Display(Name = "售价")]
        public decimal Price { get; set; }

        [Display(Name = "总销售金额")]
        public decimal Total { get; set; }

        [Display(Name = "进货价")]
        public decimal BuyingPrice { get; set; }

        [Display(Name = "差价")]
        public decimal Difference { get; set; }

        [Display(Name = "销售利润")]
        public decimal SalesProfit { get; set; }

        [Display(Name = "合伙人姓名")]
        public string UserName { get; set; }

        [Display(Name = "合伙人级别")]
        public Enums.UserType UserRank { get; set; }

        [Display(Name = "备注")]
        public string Remark { get; set; }

    }

    /// <summary>
    /// 直推奖明细报表
    /// </summary>
    public class DirectDetailsViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 销售者
        /// </summary>
        [Display(Name = "销售者")]
        public string Seller { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        [Display(Name = "销售数量")]
        public int Count { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        [Display(Name = "售价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 总销售金额
        /// </summary>
        [Display(Name = "总销售金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 直推奖金10%
        /// </summary>
        [Display(Name = "直推奖金10%")]
        public decimal Bonus { get; set; }

        /// <summary>
        /// 付款账户
        /// </summary>
        [Display(Name = "付款账户")]
        public string Pay { get; set; }

        /// <summary>
        /// 收款账户
        /// </summary>
        [Display(Name = "收款账户")]
        public string Receivable { get; set; }

        /// <summary>
        /// 收款人账户名称
        /// </summary>
        [Display(Name = "收款人账户名称")]
        public string ReceivableName { get; set; }

        /// <summary>
        /// 收款网点联行号
        /// </summary>
        [Display(Name = "收款网点联行号")]
        public string ReceivableNumber { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        [Display(Name = "交易金额")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 附言
        /// </summary>
        [Display(Name = "附言")]
        public string Remark { get; set; }

        /// <summary>
        /// 收款人级别
        /// </summary>
        [Display(Name = "收款人级别")]
        public Enums.UserType ReceivableRank { get; set; }

        /// <summary>
        /// 收款人身份证号
        /// </summary>
        [Display(Name = "收款人身份证号")]
        public string ReceivablesIDCard { get; set; }

        /// <summary>
        /// 已付款
        /// </summary>
        [Display(Name = "已付款")]
        public bool IsPay { get; set; }
    }

    /// <summary>
    /// 直推奖付款报表
    /// </summary>
    public class DirectPaymentViewModel
    {
        /// <summary>
        /// 序号
        /// </summary>
        [Display(Name = "序号")]
        public int ID { get; set; }

        /// <summary>
        /// 付款账户
        /// </summary>
        [Display(Name = "付款账户")]
        public string Pay { get; set; }

        /// <summary>
        /// 收款人账户名称
        /// </summary>
        [Display(Name = "收款人账户名称")]
        public string ReceivableName { get; set; }

        /// <summary>
        /// 收款人银行名称
        /// </summary>
        [Display(Name = "收款人银行名称")]
        public string ReceivableBankName { get; set; }

        /// <summary>
        /// 收款网点联行号
        /// </summary>
        [Display(Name = "收款网点联行号")]
        public string ReceivableNumber { get; set; }

        /// <summary>
        /// 收款人银行卡号
        /// </summary>
        [Display(Name = "收款人银行卡号")]
        public string ReceivableCard { get; set; }
        
        /// <summary>
        /// 交易金额
        /// </summary>
        [Display(Name = "交易金额")]
        public decimal Totla { get; set; }

        /// <summary>
        /// 附言
        /// </summary>
        [Display(Name = "附言")]
        public string Remark { get; set; }

        /// <summary>
        /// 购买订单用户
        /// </summary>
        [Display(Name = "购买订单用户")]
        public string OrderUser { get; set; }
    }

    /// <summary>
    /// 进货报表
    /// </summary>
    public class StorageViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        public string Code { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        public string SupplierName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 进货数量
        /// </summary>
        [Display(Name = "进货数量")]
        public int Count { get; set; }

        /// <summary>
        /// 进货价
        /// </summary>
        [Display(Name = "进货价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 进货人
        /// </summary>
        [Display(Name = "进货人")]
        public string Name { get; set; }

        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 进货人级别
        /// </summary>
        [Display(Name = "进货人级别")]
        public Enums.UserType? Rank { get; set; }

        /// <summary>
        /// 上级合伙人级别
        /// </summary>
        [Display(Name = "上级合伙人级别")]
        public Enums.UserType? ParentRank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 未发货订单明细
    /// </summary>
    public class SendOrderViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [Display(Name = "订单号")]
        public string Code { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        [Display(Name = "发货人")]
        public string Parent { get; set; }

        /// <summary>
        /// 发货人级别
        /// </summary>
        [Display(Name = "发货人级别")]
        public Enums.UserType? ParentRank { get; set; }

        /// <summary>
        /// 进货人
        /// </summary>
        [Display(Name = "进货人")]
        public string User { get; set; }

        /// <summary>
        /// 进货人级别
        /// </summary>
        [Display(Name = "进货人级别")]
        public Enums.UserType UserRank { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        [Display(Name = "订单数量")]
        public int Count { get; set; }

        /// <summary>
        /// 已发数量
        /// </summary>
        [Display(Name = "已发数量")]
        public int Send { get; set; }

        /// <summary>
        /// 未发数量
        /// </summary>
        [Display(Name = "未发数量")]
        public int IsSend { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        [Display(Name = "单价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        [Display(Name = "订单总金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 已发货金额
        /// </summary>
        [Display(Name = "已发货金额")]
        public decimal SendTotal { get; set; }

        /// <summary>
        /// 未发货金额
        /// </summary>
        [Display(Name = "未发货金额")]
        public decimal IsSendTotal { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 销售利润报表
    /// </summary>
    public class SalesProfitViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Display(Name = "产品名称")]
        public string ProductName { get; set; }

        /// <summary>
        /// 销售者
        /// </summary>
        [Display(Name = "销售者")]
        public string Parent { get; set; }

        /// <summary>
        /// 销售数量
        /// </summary>
        [Display(Name = "销售数量")]
        public int Count { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        [Display(Name = "售价")]
        public decimal Price { get; set; }

        /// <summary>
        /// 总销售金额
        /// </summary>
        [Display(Name = "总销售金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 进货价
        /// </summary>
        [Display(Name = "进货价")]
        public decimal BuyingPrice { get; set; }

        /// <summary>
        /// 差价
        /// </summary>
        [Display(Name = "差价")]
        public decimal Difference { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        [Display(Name = "利润")]
        public decimal Profit { get; set; }

        /// <summary>
        /// 合伙人姓名
        /// </summary>
        [Display(Name = "合伙人姓名")]
        public string User { get; set; }

        /// <summary>
        /// 合伙人级别
        /// </summary>
        [Display(Name = "合伙人级别")]
        public Enums.UserType UserRank { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }

        public bool IsOrder { get; set; }
    }

    /// <summary>
    /// 公司利润报表
    /// </summary>
    public class ProfitViewModel
    {
        /// <summary>
        /// 项次
        /// </summary>
        [Display(Name = "项次")]
        public int ID { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        [Display(Name = "项目")]
        public string AccountKind { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [Display(Name = "说明")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 公司股东分红报表
    /// </summary>
    public class ShareHolder
    {
        /// <summary>
        /// 项次
        /// </summary>
        [Display(Name = "项次")]
        public int ID { get; set; }

        /// <summary>
        /// 股东名称
        /// </summary>
        [Display(Name = "股东名称")]
        public string Realname { set; get; }

        /// <summary>
        /// 银行名称
        /// </summary>
        [Display(Name = "银行名称")]
        public string BankName { set; get; }

        /// <summary>
        /// 银行卡号
        /// </summary>
        [Display(Name = "银行卡号")]
        public string BankCard { set; get; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [Display(Name = "身份证号")]
        public string IDCard { set; get; }

        /// <summary>
        /// 所占股份
        /// </summary>
        [Display(Name = "所占股份")]
        public int Stock { get; set; }

        /// <summary>
        /// 分红金额
        /// </summary>
        [Display(Name = "分红金额")]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// 帐户资金进出结存报表
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// 交期日期
        /// </summary>
        [Display(Name = "交期日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Time { get; set; }

        /// <summary>
        /// 交易人
        /// </summary>
        [Display(Name = "交易人")]
        public string Trader { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        [Display(Name = "级别")]
        public Enums.UserType? Rank { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        [Display(Name = "金额")]
        public decimal Total { get; set; }

        /// <summary>
        /// 公司结存
        /// </summary>
        [Display(Name = "公司结存")]
        public decimal Amount { get; set; }

        /// <summary>
        /// 公司银行
        /// </summary>
        [Display(Name = "公司银行")]
        public string BankName { get; set; }

        /// <summary>
        /// 公司帐户
        /// </summary>
        [Display(Name = "公司帐户")]
        public string BankCard { get; set; }

        /// <summary>
        /// 交易人类型
        /// </summary>
        [Display(Name = "交易人类型")]
        public Enums.TraderType TraderType { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        [Display(Name = "项目")]
        public AccountKind AccountKind { get; set; }

        /// <summary>
        /// 已取消
        /// </summary>
        [Display(Name = "已取消")]
        public bool IsDelete { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        public string Remark { get; set; }
    }

    /// <summary>
    /// 银行资金总额分布报表
    /// </summary>
    public class BankCapital
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Display(Name = "账号")]
        public string BankAccount { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        [Display(Name = "银行")]
        public string BankName { get; set; }

        /// <summary>
        /// 帐户余额
        /// </summary>
        [Display(Name = "帐户余额")]
        public decimal Total { get; set; }
    }
}