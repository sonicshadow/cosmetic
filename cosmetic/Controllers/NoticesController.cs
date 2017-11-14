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
    public class NoticesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "公告管理";
        }

        // GET: Notices
        [Authorize(Roles =SysRole.NoticesRead)]
        public ActionResult Index(int page=1)
        {
            Sidebar();
            var paged = db.Notices.OrderByDescending(s => s.CreateTime).ToPagedList(page);
            return View(paged);
        }

        [AllowAnonymous]
        // GET: Notices/Details/5
        public ActionResult Details(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // GET: Notices/Create
        [Authorize(Roles = SysRole.NoticesRead)]
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        // POST: Notices/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.NoticesRead)]
        public ActionResult Create(Notice notice)
        {
            Sidebar();
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(notice.Title))
                {
                    ModelState.AddModelError("", "标题 字段是必需的。");
                    return View(notice);
                }
                db.Notices.Add(notice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // GET: Notices/Edit/5
        [Authorize(Roles = SysRole.NoticesEdit)]
        public ActionResult Edit(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // POST: Notices/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.NoticesEdit)]
        public ActionResult Edit(Notice notice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            return View(notice);
        }

        // GET: Notices/Delete/5
        [Authorize(Roles = SysRole.NoticesDelete)]
        public ActionResult Delete(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Notice notice = db.Notices.Find(id);
            if (notice == null)
            {
                return RedirectToAction("Index");
            }
            return View(notice);
        }

        // POST: Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.NoticesDelete)]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = db.Notices.Find(id);
            db.Notices.Remove(notice);
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
