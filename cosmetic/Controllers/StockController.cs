using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "库存管理";
        }

        // GET: Stock
        public ActionResult Index(int pid, string name, int page = 1)
        {
            Sidebar();
            var list = GetStock(pid, name).AsQueryable().ToPagedList(page);
            return View(list);
        }

        public ActionResult IndexExport(int pid, string name, int page = 1)
        {
            var list = GetStock(pid, name).AsQueryable().ToPagedList(page);
            this.AddExcelExportHead($"库存交易报表{DateTime.Now:yyyyMMdd}");
            return View(list);
        }

        public List<StockViewModel> GetStock(int pid, string name)
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            var list = new List<StockViewModel>();
            if (string.IsNullOrWhiteSpace(name))
            {
                var orders = db.Orders.Where(s => s.ParentUser == null &&
                    s.State != Enums.OrderState.Delete &&
                    s.State != Enums.OrderState.UnPay).ToList();
                var userids = orders.Select(s => s.UserID);
                var stocks = db.Stock.Where(s => (userids.Contains(s.UserID) ||
                    s.UserID == null ||
                    s.Type == Enums.StockType.Replacement));
                stocks = stocks.Where(s => s.ProductID == pid);
                var uids = stocks.Where(s => s.UserID != null).Select(s => s.UserID);
                users = db.Users.Where(s => uids.Contains(s.Id)).ToList();
                var sp = db.SupplierProducts.Include(s => s.Supplier).Where(s => s.ProductID == pid).ToList();
                foreach (var item in stocks.ToList())
                {
                    if (item.UserID != null && item.Type != Enums.StockType.Replacement && item.Type != Enums.StockType.AddStock && item.Type != Enums.StockType.Deliver)
                    {
                    }
                    else
                    {
                        var u = users.FirstOrDefault(s => s.Id == item.UserID);
                        string no = null, supplier = null;
                        if (string.IsNullOrWhiteSpace(item.UserID))
                        {
                            var su = sp.FirstOrDefault(s => s.ID == item.DataID);
                            if (su != null)
                            {
                                supplier = su.Supplier.Name;
                                no = su.Code;
                            }
                        }
                        else
                        {
                            var order = orders.FirstOrDefault(s => s.ID == item.DataID);
                            no = order == null ? "" : order.Code;
                            supplier = u.RealName;
                        }
                        item.Count = item.Type == Enums.StockType.Replacement ? -item.Count : item.Count;
                        list.Add(new StockViewModel(item, u, supplier, no));
                    }
                };
            }
            else
            {
                name = name.Trim();
                var user = db.Users.FirstOrDefault(s => s.UserName == name);
                if (user != null)
                {
                    list = UserDetails(user.Id).Where(s => s.ProductID == pid).ToList();
                }
            }
            return list;
        }

        public ActionResult UserIndex(int pid = 0, int page = 1)
        {
            Sidebar();
            var list = UserDetails(UserID);
            var show = true;
            if (pid != 0)
            {
                list = list.Where(s => s.ProductID == pid).ToList();
                var shoew = list.Where(s => s.Type == Enums.StockType.Use).OrderBy(s => s.CreateTime).FirstOrDefault();
                if (shoew != null && shoew.CreateTime.Year == DateTime.Now.Year)
                {
                    show = false;
                }
            }
            ViewBag.Product = db.Products.ToList().Select(s => new SelectListItem()
            {
                Selected = s.ID == pid,
                Text = s.Name,
                Value = s.ID.ToString(),
            }).ToList();
            if (list.Sum(s => s.Count) <= 0)
            {
                show = false;
            }
            ViewBag.Show = show;
            var paged = list.AsQueryable().ToPagedList(page);
            return View(paged);
        }

        public List<StockViewModel> UserDetails(string name)
        {
            var list = new List<StockViewModel>();
            var orders = db.Orders.Where(s => (s.ParentUser == name || s.UserID == name) &&
                s.State != Enums.OrderState.Delete &&
                s.State != Enums.OrderState.UnPay).ToList();
            var userids = orders.Select(s => s.UserID).Distinct();
            var stock = db.Stock.Include(s => s.Product)
                .Where(s => userids.Contains(s.UserID));
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            foreach (var item in stock)
            {
                if (item.UserID != name && item.Type != Enums.StockType.AddStock && item.Type != Enums.StockType.Deliver)
                {
                }
                else if (!orders.Select(s => s.ID).Contains(item.DataID) && item.Type == Enums.StockType.Deliver)
                {
                }
                else
                {
                    var u = users.FirstOrDefault(s => s.Id == item.UserID);
                    if (item.UserID == name && item.Type == Enums.StockType.Deliver)
                    {
                        item.Type = Enums.StockType.AddStock;
                        item.Count = -item.Count;
                    }
                    var o = orders.FirstOrDefault(s => s.ID == item.DataID);
                    var no = o == null ? "" : o.Code;
                    string supplier = null;
                    if (o != null)
                    {
                        if (o.ParentUser == null)
                        {
                            supplier = "公司";
                        }
                        else
                        {
                            supplier = u == null ? "" : u.RealName;
                        }
                    }
                    if (item.UserID == name && item.Type == Enums.StockType.Replacement)
                    {
                        supplier = u == null ? "" : u.RealName;
                    }
                    list.Add(new StockViewModel(item, u, supplier, no));
                }
            };
            return list;
        }

        [HttpPost]
        public ActionResult Crear(int pid)
        {
            var stocks = db.Stock.Where(s => s.UserID == UserID &&
                s.Type == Enums.StockType.Use &&
                s.ProductID == pid &&
                s.CreateTime.Year == DateTime.Now.Year);
            if (stocks.Count() > 0)
            {
                return Json(Comm.ToMobileResult("Error", "出货功能今年已使用过一次了"));
            }
            var stock = new Stock()
            {
                Count = -1,
                CreateTime = DateTime.Now,
                ProductID = pid,
                DataID = 0,
                Type = Enums.StockType.Use,
                Remark = "自己做脸使用",
                UserID = UserID
            };
            db.Stock.Add(stock);
            var userProduct = db.UserProducts.FirstOrDefault(s => s.ProductID == pid && s.UserID == UserID);
            userProduct.Count = userProduct.Count - 1;
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "成功"));
        }
    }
}
