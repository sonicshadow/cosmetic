using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Microsoft.AspNet.Identity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class ReturnsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "退货管理";
            return;
        }

        // GET: Returns
        [Authorize(Roles = SysRole.ReturnRead)]
        public ActionResult Index(Enums.CheckState? state, int page = 1)
        {
            Sidebar();
            var returns = db.Returns.Include(s => s.Order).Include(s => s.Product).Include(s=>s.Order.User);
            ViewBag.NoCheckCount = returns.Count(s => s.CheckState == Enums.CheckState.NoCheck);
            if (state.HasValue)
            {
                returns = returns.Where(s => s.CheckState == state);
            }
            var paged = returns.OrderBy(s => s.Time).ToPagedList(page);
            var ids = paged.Select(s => s.User).ToList();
            ids.AddRange(paged.Select(s => s.CheckUser).ToList());
            ids.Distinct();
            var users = db.Users.Where(s => ids.Contains(s.Id)).ToList();
            foreach (var item in paged)
            {
                var user = users.FirstOrDefault(s => s.Id == item.User);
                item.User = user.UserName;
                var checkuser = users.FirstOrDefault(s => s.Id == item.CheckUser);
                item.CheckUser = checkuser == null ? "" : checkuser.UserName;
            }
            return View(paged);
        }

        // GET: Returns/Create
        [Authorize(Roles = SysRole.ReturnCreate)]
        public ActionResult Create()
        {
            Sidebar();
            ViewBag.NewProductID = new SelectList(db.Products, "ID", "Name");
            var model = new ReturnViewModel()
            {
                Count = 0,
            };
            return View(model);
        }

        // POST: Returns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.ReturnCreate)]
        public ActionResult Create(ReturnViewModel r)
        {
            ViewBag.NewProductID = new SelectList(db.Products, "ID", "Name", r.NewProductID);
            var order = db.Orders.FirstOrDefault(s => s.Code == r.Order.Code);
            if (order == null)
            {
                ModelState.AddModelError("Order.Code", "没有找到订单");
                return View(r);
            }
            if (order.Send < r.Count)
            {
                ModelState.AddModelError("Order.Code", "订单未收货不能申请退换货");
                
                return View(r);
            }
            var model = new Return()
            {
                User = UserID,
                Time = DateTime.Now,
                OrderID = r.Order.ID,
                CheckState = Enums.CheckState.NoCheck,
                Price = r.Price,
                ProductID = r.NewProductID,
                Total = r.Total,
                Count = r.Count,
            };
            db.Returns.Add(model);
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(r);
            }
            return RedirectToAction("Index");
        }

        // GET: Returns/Edit/5
        public ActionResult Edit(int id)
        {
            Sidebar();
            Return r = db.Returns
                .Include(s => s.Order)
                .Include(s => s.Product)
                .Include(s => s.Order.User)
                .FirstOrDefault(s => s.ID == id);
            if (r == null)
            {
                return RedirectToAction("Index");
            }
            var users = db.Users.Where(s => s.Id == r.User || s.Id == r.CheckUser);
            var user = users.FirstOrDefault(s => s.Id == r.User);
            var checkUser = users.FirstOrDefault(s => s.Id == r.CheckUser);
            var oldproduct = db.Products.FirstOrDefault(s => s.ID == r.Order.ProductID);
            r.Order.Products = oldproduct;
            r.User = user.UserName;
            r.CheckUser = checkUser == null ? null : checkUser.UserName;
            if (r.CheckState == Enums.CheckState.Pass && !r.PayTime.HasValue)
            {
                ViewBag.Payee = Bll.Account.GetPayeeInSelect();
            }
            var model = new ReturnViewModel(r);
            return View(model);
        }

        [HttpPost]
        public ActionResult Check(int id, bool result)
        {
            Return r = db.Returns.Include(s => s.Order)
                 .Include(s => s.Product)
                 .Include(s => s.Order.User)
                 .FirstOrDefault(s => s.ID == id);
            if (r.CheckState != Enums.CheckState.NoCheck)
            {
                return Json(Comm.ToMobileResult("Error", "申请已通过审核"));
            }
            if (result)
            {
                var userProduct = db.UserProducts.FirstOrDefault(s => s.ProductID == r.Order.ProductID && s.UserID == r.Order.UserID);
                userProduct.Sum = userProduct.Sum - r.Count;
                userProduct.Count = userProduct.Count - r.Count;
                if (userProduct.Count < 0)
                {
                    return Json(Comm.ToMobileResult("Error", "用户还没收到货"));
                }
            }
            r.CheckState = result ? Enums.CheckState.Pass : Enums.CheckState.NoPass;
            r.CheckTime = DateTime.Now;
            r.CheckUser = UserID;
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "审核完成"));
        }

        [HttpPost]
        public ActionResult Receivables(int id, int payeeID, decimal fee)
        {
            Return r = db.Returns.Include(s => s.Order)
                .Include(s => s.Product)
                .Include(s => s.Order.User)
                .FirstOrDefault(s => s.ID == id);
            if (r.PayTime.HasValue)
            {
                return Json(Comm.ToMobileResult("Error", "申请已收款"));
            }
            r.PayTime = DateTime.Now;
            var payee = db.Payees.FirstOrDefault(s => s.ID == payeeID);
            var account = new Account()
            {
                Fee = fee,
                Remark = $"{r.Order.User.RealName}换货收款",
                Amount = r.Total-(r.Order.Price*r.Count)<0?0: r.Total - (r.Order.Price * r.Count),
                Trader = r.Order.UserID,
                TraderType = Enums.TraderType.User,
                UpdateDateTime = DateTime.Now,
                UserType = r.Order.User.Rank,
            };
            try
            {
                Bll.Account.Create(account, "商品类收入", payeeID);
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Error", ex.Message));
            }
            finiish(r);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "收款完成"));
        }

        [HttpPost]
        public ActionResult Receipt(int id)
        {
            Return r = db.Returns.Include(s => s.Order)
                .Include(s => s.Product)
                .Include(s => s.Order.User)
                .FirstOrDefault(s => s.ID == id);
            if (r.ReceiptTime.HasValue)
            {
                return Json(Comm.ToMobileResult("Error", "申请已收货"));
            }
            r.ReceiptTime = DateTime.Now;
            var supplierProduct = db.SupplierProducts.Where(s => s.ProductID == r.Order.ProductID &&
                s.Send >= r.Count && s.Count >= r.Count);
            var sp = supplierProduct.OrderByDescending(s => s.CreateTime).First();
            sp.Remaining = sp.Remaining + r.Count;
            var stock = new Stock()
            {
                Count = -r.Count,
                CreateTime = DateTime.Now,
                ProductID = r.Order.ProductID,
                Remark = $"{r.Order.User.RealName}换货的退货",
                Type = Enums.StockType.Replacement,
                UserID = r.Order.UserID
            };
            db.Stock.Add(stock);
            finiish(r);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "审核完成"));
        }

        public void finiish(Return r)
        {
            if (r.PayTime.HasValue && r.ReceiptTime.HasValue)
            {
                var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.ID == r.OrderID);
                order.Count = order.Count - r.Count;
                order.Send = order.Send - r.Count;
                if (order.Send >= order.Count)
                {
                    order.State = Enums.OrderState.Finish;
                }
                var neworder = new Order()
                {
                    Code = $"{DateTime.Now.ToString("yyyyMMdd")}{Comm.Random.Next(1000, 9999)}",
                    Count = r.Count,
                    CreateDateTime = DateTime.Now,
                    Send = 0,
                    Price = r.Price,
                    IsPay = true,
                    Total = r.Total,
                    ReceiptDateTime = r.PayTime,
                    UserID = order.UserID,
                    ProductID = r.ProductID,
                    State = Enums.OrderState.Pay,
                    ParentUser = null,
                };
                db.Orders.Add(neworder);
                db.SaveChanges();
            }
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
