using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class InsideUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "管理员管理";
        }

        // GET: InsideUsers
        [Authorize(Roles =SysRole.AdminManageRead)]
        public ActionResult Index(int page = 1)
        {
            Sidebar();
            var user = db.Users.Where(s => s.Type == Enums.User.Admin);
            var roleIds = user.Select(s => s.RoleGroupID);
            var roles = db.RoleGroups.Where(s => roleIds.Contains(s.ID));
            foreach (var item in user.ToList())
            {
                var rolesName = roles.FirstOrDefault(s => s.ID == item.RoleGroupID);
                item.RoleGroup = rolesName.Name;
            }
            var model = user.OrderBy(s => s.RegisterDateTime).ToPagedList(page);
            return View(model);
        }

        // GET: InsideUsers/Details/5
        [Authorize(Roles = SysRole.AdminManageRead)]
        public ActionResult Details(string id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: InsideUsers/Edit/5
        [Authorize(Roles = SysRole.AdminManageEdit)]
        public ActionResult Edit(string id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return RedirectToAction("Index");
            }
            var role = db.RoleGroups.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.ID.ToString()
            }).ToList();
            ViewBag.roleGroup = role;
            return View(applicationUser);
        }

        // POST: InsideUsers/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AdminManageEdit)]
        public ActionResult Edit(ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.Find(model.Id);
                user.RealName = model.RealName;
                user.Address = model.Address;
                user.Bank = model.Bank;
                user.BankCard = model.BankCard;
                user.IDCard = model.IDCard;
                user.PhoneNumber = model.PhoneNumber;
                user.RoleGroupID = model.RoleGroupID;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.roleGroup = db.RoleGroups.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.ID.ToString()
            }).ToList();
            return View(model);
        }

        // GET: InsideUsers/Delete/5
        [Authorize(Roles = SysRole.AdminManageDelete)]
        public ActionResult Delete(string id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return RedirectToAction("Index");
            }
            var role = db.RoleGroups.Find(applicationUser.RoleGroupID).Name;
            applicationUser.RoleGroup = role;
            return View(applicationUser);
        }

        // POST: InsideUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SysRole.AdminManageDelete)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
