using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosmetic
{
    public class SysRole
    {
        #region 用户管理
        /// <summary>
        /// 用户查看
        /// </summary>
        public const string UserManageRead = "UserManageRead";
        /// <summary>
        /// 用户添加
        /// </summary>
        public const string UserManageCreate = "UserManageCreate";
        /// <summary>
        /// 用户修改
        /// </summary>
        public const string UserManageEdit = "UserManageEdit";
        /// <summary>
        /// 用户删除
        /// </summary>
        public const string UserManageDelete = "UserManageDelete";
        #endregion
        
        #region 商品管理
        /// <summary>
        /// 商品查看
        /// </summary>
        public const string ProductManageRead = "ProductManageRead";
        /// <summary>
        /// 商品添加
        /// </summary>
        public const string ProductManageCreate = "ProductManageCreate";
        /// <summary>
        /// 商品修改
        /// </summary>
        public const string ProductManageEdit = "ProductManageEdit";
        /// <summary>
        /// 商品删除
        /// </summary>
        public const string ProductManageDelete = "ProductManageDelete";
        #endregion
        
        #region 供应商管理
        /// <summary>
        /// 供应商查看
        /// </summary>
        public const string SupplierManageRead = "SupplierManageRead";
        /// <summary>
        /// 供应商添加
        /// </summary>
        public const string SupplierManageCreate = "SupplierManageCreate";
        /// <summary>
        /// 供应商修改
        /// </summary>
        public const string SupplierManageEdit = "SupplierManageEdit";
        /// <summary>
        /// 供应商删除
        /// </summary>
        public const string SupplierManageDelete = "SupplierManageDelete";
        #endregion
        
        #region 订单管理
        /// <summary>
        /// 订单查看
        /// </summary>
        public const string OrderManageRead = "OrderManageRead";
        /// <summary>
        /// 订单添加
        /// </summary>
        public const string OrderManageCreate = "OrderManageCreate";
        /// <summary>
        /// 订单修改
        /// </summary>
        public const string OrderManageEdit = "OrderManageEdit";
        /// <summary>
        /// 订单删除
        /// </summary>
        public const string OrderManageDelete = "OrderManageDelete";
        #endregion
        
        #region 员工管理
        /// <summary>
        /// 员工查看
        /// </summary>
        public const string StaffManageRead = "StaffManageRead";
        /// <summary>
        /// 员工添加
        /// </summary>
        public const string StaffManageCreate = "StaffManageCreate";
        /// <summary>
        /// 员工修改
        /// </summary>
        public const string StaffManageEdit = "StaffManageEdit";
        /// <summary>
        /// 员工删除
        /// </summary>
        public const string StaffManageDelete = "StaffManageDelete";
        #endregion
        
        #region 任务管理
        /// <summary>
        /// 任务查看
        /// </summary>
        public const string MissionManageRead = "MissionManageRead";
        /// <summary>
        /// 任务审核
        /// </summary>
        public const string MissionManageCheck = "MissionManageCheck";
        #endregion
        
        #region 资金管理
        /// <summary>
        /// 资金查看
        /// </summary>
        public const string AccountManageRead = "AccountManageRead";
        /// <summary>
        /// 资金添加
        /// </summary>
        public const string AccountManageCreate = "AccountManageCreate";
        /// <summary>
        /// 资金修改
        /// </summary>
        public const string AccountManageEdit = "AccountManageEdit";
        /// <summary>
        /// 资金删除
        /// </summary>
        public const string AccountManageDelete = "AccountManageDelete";
        #endregion
        
        #region 权限管理
        /// <summary>
        /// 权限查看
        /// </summary>
        public const string RoleManageRead = "RoleManageRead";
        /// <summary>
        /// 权限添加
        /// </summary>
        public const string RoleManageCreate = "RoleManageCreate";
        /// <summary>
        /// 权限修改
        /// </summary>
        public const string RoleManageEdit = "RoleManageEdit";
        /// <summary>
        /// 权限删除
        /// </summary>
        public const string RoleManageDelete = "RoleManageDelete";
        #endregion

        #region 科目管理
        /// <summary>
        /// 科目查看
        /// </summary>
        public const string AccountKindRead = "AccountKindRead";
        /// <summary>
        /// 科目添加
        /// </summary>
        public const string AccountKindCreate = "AccountKindCreate";
        /// <summary>
        /// 科目修改
        /// </summary>
        public const string AccountKindEdit = "AccountKindEdit";
        /// <summary>
        /// 科目删除
        /// </summary>
        public const string AccountKindDelete = "AccountKindDelete";
        #endregion

        #region 部门管理
        /// <summary>
        /// 部门查看
        /// </summary>
        public const string DepartmentManageRead = "DepartmentManageRead";
        /// <summary>
        /// 部门添加
        /// </summary>
        public const string DepartmentManageCreate = "DepartmentManageCreate";
        /// <summary>
        /// 部门修改
        /// </summary>
        public const string DepartmentManageEdit = "DepartmentManageEdit";
        /// <summary>
        /// 部门删除
        /// </summary>
        public const string DepartmentManageDelete = "DepartmentManageDelete";
        #endregion

        #region 管理员管理
        /// <summary>
        /// 管理员查看
        /// </summary>
        public const string AdminManageRead = "AdminManageRead";
        /// <summary>
        /// 管理员添加
        /// </summary>
        public const string AdminManageCreate = "AdminManageCreate";
        /// <summary>
        /// 管理员修改
        /// </summary>
        public const string AdminManageEdit = "AdminManageEdit";
        /// <summary>
        /// 管理员删除
        /// </summary>
        public const string AdminManageDelete = "AdminManageDelete";
        #endregion

        #region 库存管理
        /// <summary>
        /// 库存查看
        /// </summary>
        public const string StockManageRead = "StockManageRead";
        /// <summary>
        /// 库存添加
        /// </summary>
        public const string StockManageCreate = "StockManageCreate";
        /// <summary>
        /// 库存修改
        /// </summary>
        public const string StockManageEdit = "StockManageEdit";
        /// <summary>
        /// 库存删除
        /// </summary>
        public const string StockManageDelete = "StockManageDelete";
        #endregion

        #region 公司账号管理
        /// <summary>
        /// 查看收款人信息
        /// </summary>
        public const string PayeeManageRead = "PayeeManageRead";

        /// <summary>
        /// 创建收款人信息
        /// </summary>
        public const string PayeeManageCreate = "PayeeManageCreate";

        /// <summary>
        /// 编辑收款人信息
        /// </summary>
        public const string PayeeManageEdit = "PayeeManageEdit";

        /// <summary>
        /// 删除收款人信息
        /// </summary>
        public const string PayeeManageDelete = "PayeeManageDelete";
        #endregion

        #region 系统设置
        /// <summary>
        /// 查看系统设置
        /// </summary>
        public const string SystemSettingRead = "SystemSettingRead";

        /// <summary>
        /// 编辑银行设置
        /// </summary>
        public const string SystemSettingBankEdit = "SystemSettingBankEdit";

        /// <summary>
        /// 编辑橱窗设置
        /// </summary>
        public const string SystemSettingDiaplayEdit = "SystemSettingDiaplayEdit";

        /// <summary>
        /// 编辑任务设置
        /// </summary>
        public const string SystemSettingMissionEdit = "SystemSettingMissionEdit";
        #endregion

        #region 奖励管理
        /// <summary>
        /// 奖励查看
        /// </summary>
        public const string UserIncomeRead = "UserIncomeRead";
        /// <summary>
        /// 奖励添加
        /// </summary>
        public const string UserIncomeCreate = "UserIncomeCreate";
        /// <summary>
        /// 奖励修改
        /// </summary>
        public const string UserIncomeEdit = "UserIncomeEdit";
        /// <summary>
        /// 奖励删除
        /// </summary>
        public const string UserIncomeDelete = "UserIncomeDelete";
        #endregion
        
        #region 报表管理
        /// <summary>
        /// 报表查看
        /// </summary>
        public const string ReportRead = "ReportRead";
        #endregion

        #region 公告管理
        /// <summary>
        /// 公告查看
        /// </summary>
        public const string NoticesRead = "NoticesRead";
        /// <summary>
        /// 公告添加
        /// </summary>
        public const string NoticesCreate = "NoticesCreate";
        /// <summary>
        /// 公告修改
        /// </summary>
        public const string NoticesEdit = "NoticesEdit";
        /// <summary>
        /// 公告删除
        /// </summary>
        public const string NoticesDelete = "NoticesDelete";
        #endregion

        #region 发货管理
        /// <summary>
        /// 发货查看
        /// </summary>
        public const string DeliverRead = "DeliverRead";
        /// <summary>
        /// 发货审核
        /// </summary>
        public const string DeliverCheck = "DeliverCheck";
        #endregion

        #region 收款管理
        /// <summary>
        /// 收款查看
        /// </summary>
        public const string ReceivablesRead = "ReceivablesRead";
        /// <summary>
        /// 收款审核
        /// </summary>
        public const string ReceivablesCheck = "ReceivablesCheck";
        #endregion

        #region 换货管理
        /// <summary>
        /// 换货查看
        /// </summary>
        public const string ReturnRead = "ReturnRead";
        /// <summary>
        /// 换货添加
        /// </summary>
        public const string ReturnCreate = "ReturnCreate";
        /// <summary>
        /// 换货修改
        /// </summary>
        public const string ReturnEdit = "ReturnEdit";
        /// <summary>
        /// 换货删除
        /// </summary>
        public const string ReturnDelete = "ReturnDelete";
        /// <summary>
        /// 换货审核
        /// </summary>
        public const string ReturnCheck = "ReturnCheck";
        #endregion
    }
}