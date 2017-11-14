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
    public class StaffsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "员工管理";
        }

        private void AddDep()
        {
            ViewBag.AllDep = db.Departments.ToList();
        }

        // GET: Staffs
        [Authorize(Roles =SysRole.StaffManageRead)]
        public ActionResult Index(int page = 1, string filter = null, int? depID = null)
        {
            Sidebar();
            AddDep();
            var staffs = db.Staffs.Include(s => s.Department);

            if (depID.HasValue)
            {
                staffs = staffs.Where(s => s.DepartmentID == depID.Value);
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                staffs = staffs.Where(s => s.Name.Contains(filter));
            }
            var paged = staffs.OrderBy(s => s.DepartmentID).ToPagedList(page);
            return View(paged);
        }



        // GET: Staffs/Details/5
        [Authorize(Roles = SysRole.StaffManageRead)]
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            Sidebar();
            return View(staff);
        }

        // GET: Staffs/Create
        [Authorize(Roles = SysRole.StaffManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            var staff = new Staff()
            {
                DeductSalary = 0,
                PlusSalary = 0,
                EntryTime=DateTime.Now,
            };
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Name");
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.StaffManageCreate)]
        public ActionResult Create(Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Staffs.Add(staff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Name", staff.DepartmentID);
            return View(staff);
        }

        // GET: Staffs/Edit/5
        [Authorize(Roles = SysRole.StaffManageEdit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            Sidebar();
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Name", staff.DepartmentID);
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.StaffManageEdit)]
        public ActionResult Edit(Staff staff)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            ViewBag.DepartmentID = new SelectList(db.Departments, "ID", "Code", staff.DepartmentID);
            return View(staff);
        }

        // GET: Staffs/Delete/5
        [Authorize(Roles = SysRole.StaffManageDelete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Staff staff = db.Staffs.Find(id);
            if (staff == null)
            {
                return HttpNotFound();
            }
            Sidebar();
            return View(staff);
        }

        // POST: Staffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.StaffManageDelete)]
        public ActionResult DeleteConfirmed(int id)
        {
            Staff staff = db.Staffs.Find(id);
            db.Staffs.Remove(staff);
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
