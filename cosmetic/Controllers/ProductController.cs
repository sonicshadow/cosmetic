using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Microsoft.AspNet.Identity;
using Cosmetic.Enums;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "商品管理";
        }

        // GET: Product
        public ActionResult Index(int? page = 1)
        {
            Sidebar();
            var list = db.Products.OrderBy(s => s.ID).ToPagedList(page.Value);
            ViewBag.Page = list;
            return View(list);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Sidebar();
            var userProduct = db.UserProducts.Include(s => s.User).FirstOrDefault(s => s.ProductID == id && s.UserID == UserID);
            var p = db.Products.FirstOrDefault(s => s.ID == id);
            var model = new ProductViewModel(p, userProduct);
            return View(model);
        }

        // GET: Product/Create
        [Authorize(Roles = SysRole.ProductManageCreate)]
        public ActionResult Create()
        {
            Sidebar();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product model)
        {
            if (!User.IsInRole(SysRole.UserManageCreate))
            {
                return Json(Comm.ToMobileResult("Error", "账号没有权限添加"));
            }
            if (!ModelState.IsValid)
            {
                return Json(Comm.ToMobileResult("Erroe", ModelState.FirstErrorMessage()));
            }
            try
            {
                IsValidDetails(model.ProductDetail);
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Erroe", ex.Message));
            }
            var p = db.Products.Where(s => s.Number == model.Number);
            if (p.Count() > 0)
            {
                return Json(Comm.ToMobileResult("Erroe", "条形码不能重复"));
            }
            db.Products.Add(model);
            db.SaveChanges();
            Bll.UserProduct.CreateByPid(model.ID);
            return Json(Comm.ToMobileResult("Success", "添加成功"));
        }

        public void IsValidDetails(List<ProductDetail> details)
        {
            if (details == null || details.Count < 5)
            {
                throw new Exception("销售单价要填完整");
            }
            if (details.Count(s => s.UseType == UserType.One) > 1)
            {
                throw new Exception("不能填写两个一级价格");
            }
            if (details.Count(s => s.UseType == UserType.Premium) > 1)
            {
                throw new Exception("不能填写两个特级价格");
            }
            if (details.Count(s => s.UseType == UserType.Retailer) > 1)
            {
                throw new Exception("不能填写两个零售价价格");
            }
            if (details.Count(s => s.UseType == UserType.Three) > 1)
            {
                throw new Exception("不能填写两个三级价格");
            }
            if (details.Count(s => s.UseType == UserType.Two) > 1)
            {
                throw new Exception("不能填写两个二级价格");
            }
        }

        // GET: Product/Edit/5
        [Authorize(Roles = SysRole.ProductManageEdit)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var product = db.Products.FirstOrDefault(s => s.ID == id);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product model)
        {
            if (!User.IsInRole(SysRole.ProductManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", "账号没有权限"));
            }
            if (!ModelState.IsValid)
            {
                return Json(Comm.ToMobileResult("Error", ModelState.FirstErrorMessage()));
            }
            var products = db.Products.Where(s => s.ID == model.ID || s.Number == model.Number);
            var nun = products.Where(s => s.ID != model.ID && s.Number == model.Number);
            if (nun.Count() > 0)
            {
                return Json(Comm.ToMobileResult("Error", "条形码不能重复"));
            }
            try
            {
                IsValidDetails(model.ProductDetail);
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Error", ex.Message));
            }
            var p = products.FirstOrDefault(s => s.ID == model.ID);
            p.Info = model.Info;
            p.Name = model.Name;
            p.Price = model.Price;
            p.Release = model.Release;
            p.Spec = model.Spec;
            p.Unit = model.Unit;
            p.No = model.No;
            //等级修改
            var plist = p.ProductDetail.Select(s => s.ID).ToList();
            var mlist = model.ProductDetail.Select(s => s.ID).ToList();
            var edit = plist.Intersect(mlist).ToList();
            foreach (var item in edit)
            {
                var pedit = p.ProductDetail.FirstOrDefault(s => s.ID == item);
                var medit = model.ProductDetail.FirstOrDefault(s => s.ID == item);
                pedit.Min = medit.Min;
                pedit.Price = medit.Price;
                pedit.UseType = medit.UseType;
                pedit.TwiceMin = medit.TwiceMin;
            }
            var create = model.ProductDetail.Where(s => s.ID == 0);
            foreach (var item in create)
            {
                item.ProductID = model.ID;
                db.ProductDetails.Add(item);
            }
            var del = plist.Except(mlist);
            foreach (var item in del)
            {
                var pdel = p.ProductDetail.FirstOrDefault(s => s.ID == item);
                db.ProductDetails.Remove(pdel);
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }

        // GET: Product/Delete/5
        [Authorize(Roles = SysRole.ProductManageDelete)]
        public ActionResult Delete(int id)
        {
            Sidebar();
            var p = db.Products.FirstOrDefault(s => s.ID == id);
            return View(p);
        }

        // POST: Product/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = SysRole.ProductManageDelete)]
        public ActionResult DeleteConfirm(int id, FormCollection collection)
        {
            var p = db.Products.FirstOrDefault(s => s.ID == id);
            db.Products.Remove(p);
            var sp = db.SupplierProducts.Where(s => s.ProductID == id);
            foreach (var item in sp)
            {
                db.SupplierProducts.Remove(item);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult SearchByIDAndCode(int pid, string code)
        {
            var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.Code == code);
            if (order == null)
            {
                return Json(Comm.ToMobileResult("Error", "找不到数据"));
            }
            var price = db.ProductDetails.FirstOrDefault(s => s.ProductID == pid
                && s.UseType == order.User.Rank);
            if (price == null)
            {
                return Json(Comm.ToMobileResult("Error", "找不到数据"));
            }
            return Json(Comm.ToMobileResult("Success", "成功", new
            {
                price.Price
            }));
        }
    }
}
