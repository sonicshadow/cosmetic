using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Cosmetic.Controllers
{
    [Authorize]
    public class BankManageController : Controller
    {
        private void Sidebar()
        {
            ViewBag.Sidebar = "系统设置";
        }

        // GET: BankManage
        [Authorize(Roles =SysRole.SystemSettingBankEdit)]
        public ActionResult Index()
        {
            Sidebar();
            var model = Bll.SystemSettings.Banks;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string name)
        {
            if (!User.IsInRole(SysRole.SystemSettingBankEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            var model = Bll.SystemSettings.Banks;
            if (model.Any(s => s == name))
            {
                return Json(Comm.ToMobileResult("Error", $"{name}已存在"));
            }
            model.Add(name);
            return Json(Comm.ToMobileResult("Success", "成功"));
        }

        [HttpPost]
        public ActionResult Delete(string name)
        {
            if (!User.IsInRole(SysRole.SystemSettingBankEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            var model = Bll.SystemSettings.Banks;
            if (!model.Any(s => s == name))
            {
                return Json(Comm.ToMobileResult("Error", $"{name}不存在"));
            }
            model.Remove(name);
            return Json(Comm.ToMobileResult("Success", "成功"));
        }
    }
}