using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "供应商管理";
        }

        // GET: Supplier
        [Authorize(Roles = SysRole.SupplierManageRead)]
        public ActionResult Index(int? page = 1)
        {
            Sidebar();
            var list = db.Suppliers.OrderBy(s => s.ID).ToPagedList(page.Value);
            ViewBag.Page = list;
            return View(list);
        }

        // GET: Supplier/Details/5
        [Authorize(Roles = SysRole.SupplierManageRead)]
        public ActionResult Details(int id)
        {
            Sidebar();
            var supplier = db.Suppliers.FirstOrDefault(s => s.ID == id);
            return View(supplier);
        }

        // GET: Supplier/Create
        [Authorize(Roles = SysRole.SupplierManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        // POST: Supplier/Create
        [HttpPost]
        [Authorize(Roles = SysRole.SupplierManageCreate)]
        public ActionResult Create(Supplier model)
        {
            db.Suppliers.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Supplier/Edit/5
        [Authorize(Roles = SysRole.SupplierManageEdit)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var supplier = db.Suppliers.FirstOrDefault(s => s.ID == id);
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        [HttpPost]
        [Authorize(Roles = SysRole.SupplierManageEdit)]
        public ActionResult Edit(Supplier model)
        {
            var suppplier = db.Suppliers.FirstOrDefault(s => s.ID == model.ID);
            suppplier.Address = model.Address;
            suppplier.Bank = model.Bank;
            suppplier.BankCard = model.BankCard;
            suppplier.Contacts = model.Contacts;
            suppplier.Name = model.Name;
            suppplier.PhoneNumber = model.PhoneNumber;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Supplier/Delete/5
        [Authorize(Roles = SysRole.SupplierManageDelete)]
        public ActionResult Delete(int id)
        {
            Sidebar();
            var supplier = db.Suppliers.FirstOrDefault(s => s.ID == id);
            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = SysRole.SupplierManageDelete)]
        public ActionResult DeleteConfirm(int id)
        {
            var supplier=db.Suppliers.FirstOrDefault(s => s.ID == id);
            db.Suppliers.Remove(supplier);
            var sp = db.SupplierProducts.Where(s => s.SupplierID == id);
            foreach (var item in sp)
            {
                db.SupplierProducts.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}
