using Cosmetic.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class RoleManageController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        RoleManager<ApplicationRole> _roleManager;
        public RoleManager<ApplicationRole> RoleManager
        {
            get
            {
                if (_roleManager == null)
                {
                    _roleManager = new RoleManager<ApplicationRole>(new RoleStore<ApplicationRole>(db));
                }
                return _roleManager;
            }
        }

        UserManager<ApplicationUser> _userManager;
        public UserManager<ApplicationUser> UserManager
        {
            get
            {
                if (_roleManager == null)
                {
                    _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                }
                return _userManager;
            }
        }


        public void Sidebar()
        {
            ViewBag.Sidebar = "管理员管理";
        }

        // GET: RoleManage
        [Authorize(Roles = SysRole.RoleManageRead)]
        public ActionResult Index()
        {
            Sidebar();
            var modle = db.RoleGroups.OrderByDescending(s => s.ID).ToList();
            return View(modle);
        }

        // GET: RoleManage/Details/5
        public ActionResult Details(int id)
        {
            Sidebar();
            return View();
        }

        // GET: RoleManage/Create
        [Authorize(Roles = SysRole.RoleManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            RoleGroupViewModel model = new RoleGroupViewModel();
            model.RolesList.List.AddRange(GetSelectRoleView());
            return View(model);
        }

        // POST: RoleManage/Create
        [HttpPost]
        [Authorize(Roles = SysRole.RoleManageCreate)]
        public ActionResult Create(RoleGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                RoleGroup rg = new RoleGroup
                {
                    Name = model.Name,
                    Roles = model.SelectedRoles
                };
                db.RoleGroups.Add(rg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            model.RolesList.List.AddRange(GetSelectRoleView(model.SelectedRoles));
            return View(model);
        }

        // GET: RoleManage/Edit/5
        [Authorize(Roles = SysRole.RoleManageEdit)]
        public ActionResult Edit(int id)
        {
            RoleGroupViewModel model = new RoleGroupViewModel();
            var rg = db.RoleGroups.FirstOrDefault(s => s.ID == id);
            model.ID = rg.ID;
            model.Name = rg.Name;
            model.SelectedRoles = rg.Roles;
            model.RolesList.List.AddRange(GetSelectRoleView(rg.Roles));
            Sidebar();
            return View(model);
        }

        // POST: RoleManage/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.RoleManageEdit)]
        public ActionResult Edit(RoleGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                RoleGroup rg = new RoleGroup
                {
                    ID = model.ID,
                    Name = model.Name,
                    Roles = model.SelectedRoles
                };
                db.Entry(rg).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                var users = db.Users.Where(s => s.RoleGroupID == model.ID).ToList();
                var roles = rg.Roles.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                foreach (var user in users)
                {
                    user.Roles.Clear();
                }
                db.SaveChanges();
                foreach (var user in users)
                {
                    UserManager.AddToRoles(user.Id, roles);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            model.RolesList.List.AddRange(GetSelectRoleView(model.SelectedRoles));
            Sidebar();
            return View(model);
        }

        // GET: RoleManage/Delete/5
        [Authorize(Roles = SysRole.RoleManageDelete)]
        public ActionResult Delete(int id)
        {
            RoleGroupViewModel model = new RoleGroupViewModel();
            var rg = db.RoleGroups.FirstOrDefault(s => s.ID == id);
            model.ID = rg.ID;
            model.Name = rg.Name;
            model.RolesList.List.AddRange(GetSelectRoleView(rg.Roles));
            Sidebar();
            return View(model);
        }

        // POST: RoleManage/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = SysRole.RoleManageDelete)]
        public ActionResult DeleteConfirm(int id)
        {
            var rg = db.RoleGroups.FirstOrDefault(s => s.ID == id);
            db.RoleGroups.Remove(rg);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectRoleView> GetSelectRoleView(string selectedRole = "")
        {
            if (selectedRole == null)
            {
                selectedRole = "";
            }
            var roles = RoleManager.Roles.ToList();
            var lstSelectedRole = selectedRole.Split(',')
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .ToList();
            return roles.Select(s => new SelectRoleView
            {
                Selected = lstSelectedRole.Any(x => x == s.Name),
                Name = s.Name,
                Description = s.Description,
                Group = s.Group
            }).ToList();
        }


        [HttpPost]
        public ActionResult UpdateRoles()
        {
            List<ApplicationRole> sysRoles = new List<ApplicationRole>();
            Action<string, string, string> setRole = (name, group, desc) =>
            {
                sysRoles.Add(new ApplicationRole
                {
                    Name = name,
                    Group = group,
                    Description = desc,
                });
            };
            setRole(SysRole.UserManageRead, "用户管理", "用户查看");
            setRole(SysRole.UserManageCreate, "用户管理", "用户添加");
            setRole(SysRole.UserManageEdit, "用户管理", "用户修改");
            setRole(SysRole.UserManageDelete, "用户管理", "用户删除");

            setRole(SysRole.ProductManageRead, "商品管理", "商品查看");
            setRole(SysRole.ProductManageCreate, "商品管理", "商品添加");
            setRole(SysRole.ProductManageEdit, "商品管理", "商品修改");
            setRole(SysRole.ProductManageDelete, "商品管理", "商品删除");

            setRole(SysRole.SupplierManageRead, "供应商管理", "供应商查看");
            setRole(SysRole.SupplierManageCreate, "供应商管理", "供应商添加");
            setRole(SysRole.SupplierManageEdit, "供应商管理", "供应商修改");
            setRole(SysRole.SupplierManageDelete, "供应商管理", "供应商删除");

            setRole(SysRole.OrderManageRead, "订单管理", "订单查看");
            setRole(SysRole.OrderManageCreate, "订单管理", "订单添加");
            setRole(SysRole.OrderManageEdit, "订单管理", "订单修改");
            setRole(SysRole.OrderManageDelete, "订单管理", "订单删除");

            setRole(SysRole.AccountKindRead, "科目管理", "科目查看");
            setRole(SysRole.AccountKindCreate, "科目管理", "科目添加");
            setRole(SysRole.AccountKindEdit, "科目管理", "科目修改");
            setRole(SysRole.AccountKindDelete, "科目管理", "科目删除");

            setRole(SysRole.AccountManageRead, "资金管理", "资金查看");
            setRole(SysRole.AccountManageCreate, "资金管理", "资金添加");
            setRole(SysRole.AccountManageEdit, "资金管理", "资金修改");
            setRole(SysRole.AccountManageDelete, "资金管理", "资金删除");

            setRole(SysRole.MissionManageCheck, "任务管理", "任务查看");
            setRole(SysRole.MissionManageRead, "任务管理", "任务审核");

            setRole(SysRole.RoleManageRead, "权限管理", "权限查看");
            setRole(SysRole.RoleManageCreate, "权限管理", "权限添加");
            setRole(SysRole.RoleManageEdit, "权限管理", "权限修改");
            setRole(SysRole.RoleManageDelete, "权限管理", "权限删除");

            setRole(SysRole.StaffManageRead, "员工管理", "员工查看");
            setRole(SysRole.StaffManageCreate, "员工管理", "员工添加");
            setRole(SysRole.StaffManageEdit, "员工管理", "员工修改");
            setRole(SysRole.StaffManageDelete, "员工管理", "员工删除");

            setRole(SysRole.DepartmentManageRead, "部门管理", "部门查看");
            setRole(SysRole.DepartmentManageCreate, "部门管理", "部门添加");
            setRole(SysRole.DepartmentManageEdit, "部门管理", "部门修改");
            setRole(SysRole.DepartmentManageDelete, "部门管理", "部门删除");

            setRole(SysRole.AdminManageRead, "管理员管理", "管理员查看");
            setRole(SysRole.AdminManageCreate, "管理员管理", "管理员添加");
            setRole(SysRole.AdminManageEdit, "管理员管理", "管理员修改");
            setRole(SysRole.AdminManageDelete, "管理员管理", "管理员删除");

            setRole(SysRole.StockManageRead, "库存管理", "库存查看");
            setRole(SysRole.StockManageCreate, "库存管理", "库存添加");
            setRole(SysRole.StockManageEdit, "库存管理", "库存修改");
            setRole(SysRole.StockManageDelete, "库存管理", "库存删除");

            setRole(SysRole.PayeeManageRead, "收款人管理", "收款人查看");
            setRole(SysRole.PayeeManageCreate, "收款人管理", "收款人添加");
            setRole(SysRole.PayeeManageEdit, "收款人管理", "收款人修改");
            setRole(SysRole.PayeeManageDelete, "收款人管理", "收款人删除");

            setRole(SysRole.SystemSettingRead, "系统设置", "系统设置查看");
            setRole(SysRole.SystemSettingBankEdit, "系统设置", "编辑银行");
            setRole(SysRole.SystemSettingMissionEdit, "系统设置", "编辑任务");
            setRole(SysRole.SystemSettingDiaplayEdit, "系统设置", "编辑橱窗");

            setRole(SysRole.UserIncomeRead, "奖励管理", "奖励查看");
            setRole(SysRole.UserIncomeCreate, "奖励管理", "奖励添加");
            setRole(SysRole.UserIncomeEdit, "奖励管理", "奖励修改");
            setRole(SysRole.UserIncomeDelete, "奖励管理", "奖励删除");

            setRole(SysRole.NoticesRead, "公告管理", "公告查看");
            setRole(SysRole.NoticesCreate, "公告管理", "公告添加");
            setRole(SysRole.NoticesEdit, "公告管理", "公告修改");
            setRole(SysRole.NoticesDelete, "公告管理", "公告删除");

            setRole(SysRole.ReportRead, "报表管理", "报表查看");

            setRole(SysRole.DeliverRead, "发货管理", "发货查看");
            setRole(SysRole.DeliverCheck, "发货管理", "发货审核");

            setRole(SysRole.ReceivablesRead, "收款管理", "收款查看");
            setRole(SysRole.ReceivablesCheck, "收款管理", "收款审核");

            setRole(SysRole.ReturnRead, "换货管理", "换货查看");
            setRole(SysRole.ReturnEdit, "换货管理", "换货编辑");
            setRole(SysRole.ReturnCreate, "换货管理", "换货添加");
            setRole(SysRole.ReturnDelete, "换货管理", "换货删除");
            setRole(SysRole.ReturnCheck, "换货管理", "换货审核");

            var dbRoles = RoleManager.Roles.ToList();

            var sysRolesName = sysRoles.Select(s => s.Name).ToList();
            var dbSysRoleNames = dbRoles.Select(s => s.Name).ToList();
            var newSysRoleNames = sysRolesName.Except(dbSysRoleNames).ToList();

            var newRoles = sysRoles.Where(s => newSysRoleNames.Contains(s.Name)).ToList();
            if (newRoles.Count > 0)
            {
                foreach (var item in newRoles)
                {
                    RoleManager.Create(item);
                }
            }

            var delSysRoleNames = dbSysRoleNames.Except(sysRolesName).ToList();
            if (delSysRoleNames.Count > 0)
            {
                foreach (var item in dbRoles.Where(s => delSysRoleNames.Contains(s.Name)))
                {
                    RoleManager.Delete(item);
                }
            }
            return Json(new { State = "Success", Message = "更新权限成功" });
        }

    }
}
