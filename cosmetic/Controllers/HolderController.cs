using Cosmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class HolderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "用户管理";
        }

        [Authorize (Roles =SysRole.UserManageEdit)]
        // GET: Holder
        public ActionResult Index()
        {
            Sidebar();
            var holder = db.Holders.ToList();
            return View(holder);
        }

        // POST: Holder/Create
        [HttpPost]
        public ActionResult Create(Holder model)
        {
            if (!User.IsInRole(SysRole.UserManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            db.Holders.Add(model);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "添加成功"));
        }
        
        // POST: Holder/Edit/5
        [HttpPost]
        public ActionResult Edit(Holder model)
        {
            if (!User.IsInRole(SysRole.UserManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            var holder = db.Holders.FirstOrDefault(s => s.ID == model.ID);
            holder.Stock = model.Stock;
            holder.IDCard = model.IDCard;
            holder.Phone = model.Phone;
            holder.BankCard = model.BankCard;
            holder.BankName = model.BankName;
            holder.UserID = model.UserID;
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }

        // POST: Holder/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!User.IsInRole(SysRole.UserManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            var holder = db.Holders.FirstOrDefault(s=>s.ID==id);
            db.Holders.Remove(holder);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "删除成功"));
            
        }
    }
}
