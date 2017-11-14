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
    public class PayeeManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "公司账号管理";
        }

        // GET: PayeeManage
        [Authorize(Roles =SysRole.PayeeManageRead)]
        public ActionResult Index()
        {
            Sidebar();
            var payees = db.Payees.ToList();
            var list = Bll.Account.GetAccount(null, null, null, null, null);
            var amountList = list.GroupBy(s => new { s.BankName, s.BankCard }).Select(s => new BankCapital()
            {
                BankName = s.Key.BankName,
                BankAccount = s.Key.BankCard,
                Total = s.Where(w=>!w.IsDelete).Sum(q => q.Total),
            }).ToList();
            foreach (var item in payees)
            {
                var amount = amountList.FirstOrDefault(s => s.BankAccount == item.BankCard && s.BankName == item.Bank);
                item.Amount = amount == null ? 0 : amount.Total;
            }
            return View(payees);
        }

        // GET: PayeeManage/Create
        [Authorize(Roles = SysRole.PayeeManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        // POST: PayeeManage/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.PayeeManageCreate)]
        public ActionResult Create(Payee payee)
        {
            if (ModelState.IsValid)
            {
                db.Payees.Add(payee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(payee);
        }

        // GET: PayeeManage/Edit/5
        [Authorize(Roles = SysRole.PayeeManageEdit)]
        public ActionResult Edit(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return RedirectToAction("Index");
            }
            return View(payee);
        }

        // POST: PayeeManage/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.PayeeManageEdit)]
        public ActionResult Edit(Payee payee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payee);
        }

        // GET: PayeeManage/Delete/5
        [Authorize(Roles = SysRole.PayeeManageDelete)]
        public ActionResult Delete(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Payee payee = db.Payees.Find(id);
            if (payee == null)
            {
                return RedirectToAction("Index");
            }
            return View(payee);
        }

        // POST: PayeeManage/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = SysRole.PayeeManageDelete)]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payee payee = db.Payees.Find(id);
            db.Payees.Remove(payee);
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
