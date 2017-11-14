using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Cosmetic.Enums
{
    public enum User
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        [Display(Name = "普通用户")]
        Normal,
        /// <summary>
        /// 管理员
        /// </summary>
        [Display(Name = "管理员")]
        Admin,
    }

    /// <summary>
    /// 合伙人级别
    /// </summary>
    public enum UserType
    {
        /// <summary>
        /// 特级
        /// </summary>
        [Display(Name = "特级")]
        Premium,
        /// <summary>
        /// 一级
        /// </summary>
        [Display(Name = "一级")]
        One,
        /// <summary>
        /// 二级
        /// </summary>
        [Display(Name = "二级")]
        Two,
        /// <summary>
        /// 三级
        /// </summary>
        [Display(Name = "三级")]
        Three,
        /// <summary>
        /// 零售商
        /// </summary>
        [Display(Name = "零售商")]
        Retailer
    }

    public enum AccountKindType
    {
        /// <summary>
        /// 支出类
        /// </summary>
        [Display(Name = "支出类")]
        Pay,
        /// <summary>
        /// 收入类
        /// </summary>
        [Display(Name = "收入类")]
        Income,
        /// <summary>
        /// 负债类
        /// </summary>
        [Display(Name = "负债类")]
        Debt,
        /// <summary>
        /// 资产类
        /// </summary>
        [Display(Name = "资产类")]
        Assets

    }

    public enum UserIncomeType
    {
        /// <summary>
        /// 提成
        /// </summary>
        [Display(Name = "提成")]
        Bonus,
        /// <summary>
        /// 任务奖励
        /// </summary>
        [Display(Name = "任务奖励")]
        Mission,
        /// <summary>
        /// 零售提成
        /// </summary>
        [Display(Name = "零售提成")]
        RetailBonus,
    }

    public enum MissionState
    {
        /// <summary>
        /// 申请审核
        /// </summary>
        [Display(Name = "申请审核")]
        StartNoCheck,
        /// <summary>
        /// 申请不通过
        /// </summary>
        [Display(Name = "申请不通过")]
        StartNotPass,
        /// <summary>
        /// 任务进行中
        /// </summary>
        [Display(Name = "任务进行中")]
        Start,
        /// <summary>
        /// 任务失败
        /// </summary>
        [Display(Name = "任务失败")]
        Failed,
        /// <summary>
        /// 完成待审核
        /// </summary>
        [Display(Name = "完成审核")]
        CompleteNoCheck,
        /// <summary>
        /// 完成通过
        /// </summary>
        [Display(Name = "完成通过")]
        CompletePass,
        /// <summary>
        /// 完成不通过
        /// </summary>
        [Display(Name = "完成不通过")]
        CompleteNotPass
    }

    public enum MissionType
    {
        /// <summary>
        /// 一级升特级
        /// </summary>
        [Display(Name = "一级升特级")]
        LvUpOneToTop,
        /// <summary>
        /// 收款审核
        /// </summary>
        [Display(Name = "收款审核")]
        Receivables
    }

    public enum OrderState
    {
        /// <summary>
        /// 待付款
        /// </summary>
        [Display(Name = "待付款")]
        UnPay,
        /// <summary>
        /// 待发货
        /// </summary>
        [Display(Name = "待发货")]
        Pay,
        /// <summary>
        /// 发货中
        /// </summary>
        [Display(Name = "发货中")]
        Send,
        /// <summary>
        /// 订单完成
        /// </summary>
        [Display(Name = "订单完成")]
        Finish,
        /// <summary>
        /// 已取消
        /// </summary>
        [Display(Name = "已取消")]
        Delete,
    }

    public enum OrderViewState
    {
        /// <summary>
        /// 全部状态
        /// </summary>
        [Display(Name = "全部状态")]
        AllState,
        /// <summary>
        /// 待付款
        /// </summary>
        [Display(Name = "待付款")]
        UnPay,
        /// <summary>
        /// 待发货
        /// </summary>
        [Display(Name = "待发货")]
        Pay,
        /// <summary>
        /// 发货中
        /// </summary>
        [Display(Name = "发货中")]
        Send,
        /// <summary>
        /// 订单完成
        /// </summary>
        [Display(Name = "订单完成")]
        Finish,
        /// <summary>
        /// 已取消
        /// </summary>
        [Display(Name = "已取消")]
        Delete,
    }

    public enum PurchaseOrderState
    {
        /// <summary>
        /// 订单开始
        /// </summary>
        [Display(Name = "订单开始")]
        Start,
        /// <summary>
        /// 订单中间
        /// </summary>
        [Display(Name = "订单中间")]
        Ing,
        /// <summary>
        /// 订单结束
        /// </summary>
        [Display(Name = "订单结束")]
        Finish,
    }

    public enum StockType
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Display(Name = "入库")]
        AddStock,
        /// <summary>
        /// 出库
        /// </summary>
        [Display(Name = "出库")]
        Deliver,
        /// <summary>
        /// 盘盈
        /// </summary>
        [Display(Name = "盘盈")]
        Overage,
        /// <summary>
        /// 盘亏
        /// </summary>
        [Display(Name = "盘亏")]
        Loss,
        /// <summary>
        /// 自己做脸
        /// </summary>
        [Display(Name = "自己做脸")]
        Use,
        /// <summary>
        /// 换货
        /// </summary>
        [Display(Name = "换货")]
        Replacement
    }

    public enum SystemSettingType
    {
        /// <summary>
        /// 银行设置
        /// </summary>
        Banks,
        /// <summary>
        /// 橱窗设置
        /// </summary>
        [Display(Name = "橱窗设置")]
        Display,
        /// <summary>
        /// 任务设置
        /// </summary>
        [Display(Name = "任务设置")]
        Mission,
        /// <summary>
        /// 联系我们
        /// </summary>
        [Display(Name = "联系我们")]
        Contact
    }

    public enum UserState
    {
        /// <summary>
        /// 未激活
        /// </summary>
        [Display(Name = "未激活")]
        Inactive,
        /// <summary>
        /// 已激活
        /// </summary>
        [Display(Name = "已激活")]
        Actived,
        /// <summary>
        /// 冻结用户
        /// </summary>
        [Display(Name = "冻结用户")]
        Frozen
    }

    /// <summary>
    /// 占位图
    /// </summary>
    public enum DummyImage
    {
        [Display(Name = "默认")]
        Default,
        [Display(Name = "头像")]
        Avatar
    }

    public enum ResizerMode
    {
        Pad,
        Crop,
        Max,
    }

    public enum ReszieScale
    {
        Down,
        Both,
        Canvas
    }

    public enum FileType
    {
        /// <summary>
        /// 图片
        /// </summary>
        Image,
        /// <summary>
        /// 视频
        /// </summary>
        Video,
        /// <summary>
        /// 文本
        /// </summary>
        Text,
        /// <summary>
        /// 音频
        /// </summary>
        Audio,
        /// <summary>
        /// 其他
        /// </summary>
        Other
    }

    public enum TraderType
    {
        /// <summary>
        /// 其它
        /// </summary>
        [Display(Name = "其它")]
        Other,
        /// <summary>
        /// 公司账号
        /// </summary>
        [Display(Name = "公司账号")]
        Payee,
        /// <summary>
        /// 供应商
        /// </summary>
        [Display(Name = "供应商")]
        Supplier,
        /// <summary>
        /// 用户
        /// </summary>
        [Display(Name = "用户")]
        User
    }

    public enum ModifyType
    {
        /// <summary>
        /// 其它
        /// </summary>
        [Display(Name = "其它")]
        Orders,
        [Display(Name = "合伙人级别")]
        Rank,
        [Display(Name = "身份证")]
        IDCard,
        [Display(Name = "上级发货人")]
        Parent,
        [Display(Name = "电话")]
        PhoneNumber,
        [Display(Name = "地址")]
        Address,
        [Display(Name = "银行名称")]
        Bank,
        [Display(Name = "银行卡号")]
        BankCard,
        [Display(Name = "网点联行号")]
        BankCode,
        [Display(Name = "真实姓名")]
        RealName,
        [Display(Name = "推荐人")]
        Recommend,
        [Display(Name = "取消资金记录")]
        Account,
    }


    public enum CheckState
    {
        /// <summary>
        /// 通过
        /// </summary>
        [Display(Name = "通过")]
        Pass,
        /// <summary>
        /// 未通过
        /// </summary>
        [Display(Name = "未通过")]
        NoPass,
        /// <summary>
        /// 未审核
        /// </summary>
        [Display(Name = "未审核")]
        NoCheck,
    }

}