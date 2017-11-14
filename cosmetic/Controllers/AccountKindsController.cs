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
    public class AccountKindsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "资金记录";
            return;
        }

        // GET: AccountKinds
        [Authorize(Roles =SysRole.AccountKindRead)]
        public ActionResult Index()
        {
            Sidebar();
            return View(db.AccountKinds.OrderBy(s => s.Type).ThenBy(s => s.ID).ToList());
        }


        // GET: AccountKinds/Create
        [Authorize(Roles = SysRole.AccountKindCreate)]
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountKindCreate)]
        public ActionResult Create(AccountKind accountKind)
        {
            if (ModelState.IsValid)
            {
                db.AccountKinds.Add(accountKind);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            return View(accountKind);
        }

        // GET: AccountKinds/Edit/5
        [Authorize(Roles = SysRole.AccountKindEdit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountKind accountKind = db.AccountKinds.Find(id);
            if (accountKind == null)
            {
                return HttpNotFound();
            }
            return View(accountKind);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountKindEdit)]
        public ActionResult Edit(AccountKind accountKind)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountKind).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(accountKind);
        }

        // GET: AccountKinds/Delete/5
        [Authorize(Roles = SysRole.AccountKindDelete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountKind accountKind = db.AccountKinds.Find(id);
            if (accountKind == null)
            {
                return HttpNotFound();
            }
            return View(accountKind);
        }

        // POST: AccountKinds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountKindDelete)]
        public ActionResult DeleteConfirmed(int id)
        {
            AccountKind accountKind = db.AccountKinds.Find(id);
            db.AccountKinds.Remove(accountKind);
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
