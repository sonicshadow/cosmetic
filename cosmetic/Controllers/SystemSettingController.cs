using Cosmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class SystemSettingController : Controller
    {
        private void Sidebar()
        {
            ViewBag.Sidebar = "系统设置";
        }

        // GET: SystemSetting
        [Authorize(Roles = SysRole.SystemSettingRead)]
        public ActionResult Index()
        {
            Sidebar();
            return View();
        }

        [Authorize(Roles = SysRole.SystemSettingMissionEdit)]
        public ActionResult Mission(bool IsMission = true)
        {
            Sidebar();
            var mission = new MissionCompile()
            {
                Value = IsMission ? Bll.SystemSettings.Mission : Bll.SystemSettings.Contact
            };
            return View(mission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Mission(MissionCompile mission, bool IsMission = true)
        {
            if (string.IsNullOrWhiteSpace(mission.Value))
            {
                ModelState.AddModelError("Value", "内容不能为空");
                return View(mission);
            }
            if (IsMission)
            {
                Bll.SystemSettings.Mission = mission.Value;
            }
            else
            {
                Bll.SystemSettings.Contact = mission.Value;
            }
            return RedirectToAction("Index");
        }
    }
}