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
    public class DisplaysController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "系统设置";
        }

        // GET: Displays
        [Authorize(Roles =SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Index(int page = 1)
        {
            Sidebar();
            var paged = db.Displays.OrderBy(s => s.ID).ToPagedList(page);
            return View(paged);
        }

        // GET: Displays/Details/5
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            var p = db.Displays.FirstOrDefault(s => s.ID == id);
            var view = new DisplayViewModel(p);
            return View(view);
        }

        // GET: Displays/Create
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Create()
        {
            Sidebar();
            var model = new DisplayViewModel()
            {
                FileUpload = new FileUpload
                {
                    AutoInit = true,
                    Mode = FileUploadMode.Default,
                    Type = FileType.Image,
                    Name = "FileUpload",
                    Max = 9
                }
            };
            return View(model);
        }

        // POST: Displays/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Create(DisplayViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = true;
                if (string.IsNullOrWhiteSpace(model.Content))
                {
                    result = false;
                    ModelState.AddModelError("Content", "商品介绍 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Specification))
                {
                    result = false;
                    ModelState.AddModelError("Specification", "规格 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Subtitle))
                {
                    result = false;
                    ModelState.AddModelError("Subtitle", "副标题 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Tag))
                {
                    result = false;
                    ModelState.AddModelError("Tag", "标签 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    result = false;
                    ModelState.AddModelError("Title", "标题 字段是必需的。");
                }
                if (model.FileUpload.Images.Count()<=0)
                {
                    result = false;
                    ModelState.AddModelError("Images", "图片 字段是必需的。");
                }
                if (!result)
                {
                    return View(model);
                }
                var display = new Display() {
                    Content=model.Content,
                    Images=string.Join(",", model.FileUpload.Images),
                    Subtitle=model.Subtitle,
                    Tag=model.Tag,
                    Specification=model.Specification,
                    Title=model.Title
                };
                db.Displays.Add(display);
                db.SaveChanges();

                var sysDisplay = Bll.SystemSettings.Display;
                sysDisplay.Add(display.ID);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Displays/Edit/5
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Edit(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Display display = db.Displays.Find(id);
            if (display == null)
            {
                return RedirectToAction("Index");
            }
            var model = new DisplayViewModel(display);
            return View(model);
        }

        // POST: Displays/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Edit(DisplayViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = true;
                if (string.IsNullOrWhiteSpace(model.Content))
                {
                    result = false;
                    ModelState.AddModelError("Content", "商品介绍 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Specification))
                {
                    result = false;
                    ModelState.AddModelError("Specification", "规格 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Subtitle))
                {
                    result = false;
                    ModelState.AddModelError("Subtitle", "副标题 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Tag))
                {
                    result = false;
                    ModelState.AddModelError("Tag", "标签 字段是必需的。");
                }
                if (string.IsNullOrWhiteSpace(model.Title))
                {
                    result = false;
                    ModelState.AddModelError("Title", "标题 字段是必需的。");
                }
                if (model.FileUpload.Images.Count() <= 0)
                {
                    result = false;
                    ModelState.AddModelError("Images", "图片 字段是必需的。");
                }
                if (!result)
                {
                    return View(model);
                }
                var display = db.Displays.Find(model.ID);
                display.Content = model.Content;
                display.Images = string.Join(",", model.FileUpload.Images);
                display.Specification = model.Specification;
                display.Subtitle = model.Subtitle;
                display.Tag = model.Tag;
                display.Title = model.Title;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Displays/Delete/5
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult Delete(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            Display display = db.Displays.Find(id);
            if (display == null)
            {
                return RedirectToAction("Index");
            }
            var model = new DisplayViewModel(display);
            return View(model);
        }

        // POST: Displays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.SystemSettingDiaplayEdit)]
        public ActionResult DeleteConfirmed(int id)
        {
            Display display = db.Displays.Find(id);
            db.Displays.Remove(display);
            db.SaveChanges();
            var model = Bll.SystemSettings.Display;
            model.Remove(id);
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
