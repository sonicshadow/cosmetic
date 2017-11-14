using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class UserProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "我的商品";
        }

        // GET: UserProduct
        public ActionResult Index()
        {
            Sidebar();
            var up = db.UserProducts.Include(s => s.User).Include(s => s.Product)
                .Where(s => s.UserID == UserID).ToList();
            var upids = up.Where(s => s.User.Rank == Enums.UserType.One)
                .Select(s => s.ProductID)
                .ToList();
            List<Mission> missionState = new List<Mission>();
            if (upids.Count > 0)
            {
                //取出所有商品的最新任务状态
                var missionMaxIds = db.Missions
                    .Where(s => s.UserID == UserID)
                    .GroupBy(s => s.DataID)
                    .Select(s => s.Max(x => x.ID)
                    ).ToList();
                if (missionMaxIds.Count > 0)
                {
                    missionState = db.Missions.Where(s => missionMaxIds.Contains(s.ID)).ToList();
                }
            }
            ViewBag.MissionState = missionState;
            return View();
        }

        public ActionResult Team(bool isRecommend=false,int page = 1, Enums.UserType? rank = null, string filter = null)
        {
            ViewBag.Sidebar = "我的下级";
            var userProducts = db.UserProducts
                .Include(s => s.User)
                .Include(s => s.Product)
                .Where(s => s.User.AllParent.Contains(UserID) ||s.User.Parent==UserID|| s.User.Recommend == UserID);
            if (rank.HasValue)
            {
                userProducts = userProducts.Where(s => s.User.Rank == rank);
            }
            if (!string.IsNullOrWhiteSpace(filter))
            {
                userProducts = userProducts
                    .Where(s => s.User.RealName.Contains(filter)
                        || s.User.UserName == filter);
            }
            if (isRecommend)
            {
                userProducts = userProducts.Where(s => s.User.Recommend == UserID);
            }
            //取出推荐人和上级发货人用户ID
            var userIDs = userProducts.Select(s => s.User.Parent).ToList();
            userIDs.AddRange(userProducts.Select(s => s.User.Recommend));
            userIDs.Distinct();
            var users = db.Users.Where(s => userIDs.Contains(s.Id)).ToList();
            var models = new List<UserProductViewModel>();
            foreach (var item in userProducts)
            {
                var p = users.FirstOrDefault(s => s.Id == item.User.Parent);
                var r = users.FirstOrDefault(s => s.Id == item.User.Recommend);
                models.Add(new UserProductViewModel(item, p, r));
            }
            var paged = models.OrderBy(s => s.RegisterDateTime)
                .ThenBy(s=>s.RealName)
                .ThenBy(s => s.ProductID)
                .AsQueryable().ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Count = models.GroupBy(s => s.UserID).Count();
            }
            return View(paged);
        }

        public ActionResult GetPriceByPidAndUser(int pid, string user)
        {
            var up = db.UserProducts.Include(s => s.User)
                .Include(s => s.Product)
                .FirstOrDefault(s => s.ProductID == pid && s.UserID == user);
            if (up == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有相关数据"), JsonRequestBehavior.AllowGet);
            }
            var count = up.Product.ProductDetail.FirstOrDefault(s => s.UseType == up.User.Rank).Min;
            var salePrice= up.Product.ProductDetail.FirstOrDefault(s => s.UseType ==Enums.UserType.Retailer).Price;
            return Json(Comm.ToMobileResult("Success", "成功", new { price = up.Price, count = count, salePrice= salePrice }), JsonRequestBehavior.AllowGet);
        }

        // POST: UserProduct/Create
        [HttpPost]
        public ActionResult Create(UserProduct model)
        {
            var p = db.Products.FirstOrDefault(s => s.Number == model.Product.Number);
            if (p == null)
            {
                return Json(Comm.ToMobileResult("Error", "条形码有误"));
            }
            var up = db.UserProducts.Where(s => s.UserID == model.UserID && s.ProductID == p.ID).ToList();
            if (up.Count > 0)
            {
                return Json(Comm.ToMobileResult("Error", "一个商品只能添加一次"));
            }
            var userProduct = new UserProduct()
            {
                Min = model.Min,
                Price = model.Price,
                UserID = model.UserID,
                CreateTime = DateTime.Now,
                ProductID = p.ID,
            };
            db.UserProducts.Add(userProduct);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "成功"));

        }

        // POST: UserProduct/Edit/5
        [HttpPost]
        public ActionResult Edit(UserProduct model)
        {
            var userproduct = db.UserProducts.FirstOrDefault(s => s.ID == model.ID);
            userproduct.Min = model.Min;
            userproduct.TwiceMin = model.TwiceMin;
            userproduct.Price = model.Price;
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }
    }
}
