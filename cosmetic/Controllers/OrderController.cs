using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Cosmetic.Enums;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "订单管理";
        }

        // GET: Order
        public ActionResult Index(string userID, OrderViewState? state, DateTime? start,
            DateTime? end, int page = 1)
        {
            Sidebar();
            var list = GetList(userID, start, end, state);
            var model = list.AsQueryable()
                .OrderBy(s => s.CreateDateTime).ToPagedList(page);
            if (model.IsLastPage)
            {
                ViewBag.Stock = GetFoot(list);
            }
            return View(model);
        }

        public ActionResult IndexExport(string userID, OrderViewState? state, DateTime? start,
    DateTime? end, int page = 1)
        {
            var list = GetList(userID, start, end, state);
            ViewBag.Stock = GetFoot(list);
            this.AddExcelExportHead($"订单管理{DateTime.Now:yyyyMMdd}");
            return View(list);
        }

        public List<Order> GetList(string userID, DateTime? start, DateTime? end,
            OrderViewState? state)
        {
            var type = Comm.GetType(UserID);
            var order = db.Orders.Include(s => s.User).AsQueryable();
            if (type == Enums.User.Normal)
            {
                order = order.Where(s => s.ParentUser == UserID);
            }
            else
            {
                order = order.Where(s => s.ParentUser == null);
            }
            if (!string.IsNullOrWhiteSpace(userID))
            {
                userID = userID.Trim();
                order = order.Where(s => s.User.RealName == userID || s.User.RealName == userID);
            }
            if (state.HasValue)
            {
                switch (state)
                {
                    case OrderViewState.UnPay:
                        order = order.Where(s => s.State == OrderState.UnPay);
                        break;
                    case OrderViewState.Pay:
                        order = order.Where(s => s.State == OrderState.Pay);
                        break;
                    case OrderViewState.Send:
                        order = order.Where(s => s.State == OrderState.Send);
                        break;
                    case OrderViewState.Finish:
                        order = order.Where(s => s.State == OrderState.Finish);
                        break;
                    case OrderViewState.Delete:
                        order = order.Where(s => s.State == OrderState.Delete);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                order = order.Where(s => s.State != OrderState.Delete && s.State != OrderState.Finish);
            }
            if (start.HasValue)
            {
                order = order.Where(s => s.CreateDateTime >= start);
            }
            if (end.HasValue)
            {
                order = order.Where(s => s.CreateDateTime <= end);
            }
            var userids = order.Select(s => s.ParentUser).ToList();
            userids.AddRange(order.Select(s => s.User.Recommend).ToList());
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var pids = order.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            foreach (var item in order.ToList())
            {
                var parent = users.FirstOrDefault(s => s.Id == item.ParentUser);
                var recommend = users.FirstOrDefault(s => s.Id == item.User.Recommend);
                item.ParentUser = parent == null ? "公司" : parent.RealName;
                item.Recommend = recommend == null ? "公司" : recommend.RealName;
                var p = ps.FirstOrDefault(s => s.ID == item.ProductID);
                item.Products = p;
            }
            return order.ToList();
        }

        public List<OrderFoot> GetFoot(List<Order> list)
        {
            var ListCount = list.Where(s => s.State != OrderState.Delete)
               .GroupBy(s => new { s.ProductID, s.Products })
               .Select(s => new OrderFoot()
               {
                   ProductID = s.Key.ProductID,
                   Product = s.Key.Products,
                   Count = s.Sum(c => c.Count),
                   Send = s.Sum(c => c.Send),
                   Owe = s.Sum(c => c.Count) - s.Sum(c => c.Send),
                   Total = s.Sum(c => c.Total),
                   IsStock = false,
               }).ToList();

            var stock = new List<OrderFoot>();
            var type = Comm.GetType(UserID);
            if (type == Enums.User.Normal)
            {
                var userProducts = db.UserProducts.Include(s => s.Product)
                    .Where(s => s.UserID == UserID).ToList()
                    .GroupBy(s => new { s.ProductID, s.Product })
                    .Select(s => new OrderFoot()
                    {
                        ProductID = s.Key.ProductID,
                        Product = s.Key.Product,
                        Count = s.Sum(c => c.Count),
                        Send = 0,
                        Owe = 0,
                        Total = 0,
                        IsStock = true,
                    });
                stock.AddRange(userProducts);
            }
            else
            {
                var supplierProducts = db.SupplierProducts.Include(s => s.Product).GroupBy(s => new { s.ProductID, s.Product });
                foreach (var item in supplierProducts)
                {
                    stock.Add(new OrderFoot()
                    {
                        ProductID = item.Key.ProductID,
                        Product = item.Key.Product,
                        Count = item.Sum(c => c.Count),
                        Send = 0,
                        Owe = 0,
                        Total = 0,
                        IsStock = true,
                    });
                }
            }
            foreach (var item in stock)
            {
                var firet = ListCount.FirstOrDefault(s => s.ProductID == item.ProductID);
                int count = firet == null ? 0 : firet.Owe;
                item.Owe = item.Count - count;
            }
            ListCount.AddRange(stock);
            return ListCount;
        }

        public ActionResult UserOrder(string link, OrderViewState? state, int page = 1)
        {
            Sidebar();
            IEnumerable<Order> orders = null;
            if (link == "DirectPush")
            {
                orders = db.Orders
               .Include(s => s.User)
               .Where(s => s.User.Recommend == UserID);
            }
            else
            {
                orders = db.Orders
               .Include(s => s.User)
               .Where(s => s.UserID == UserID);
            }
            if (state.HasValue)
            {
                switch (state)
                {
                    case OrderViewState.UnPay:
                        orders = orders.Where(s => s.State == OrderState.UnPay);
                        break;
                    case OrderViewState.Pay:
                        orders = orders.Where(s => s.State == OrderState.Pay);
                        break;
                    case OrderViewState.Send:
                        orders = orders.Where(s => s.State == OrderState.Send);
                        break;
                    case OrderViewState.Finish:
                        orders = orders.Where(s => s.State == OrderState.Finish);
                        break;
                    case OrderViewState.Delete:
                        orders = orders.Where(s => s.State == OrderState.Delete);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                orders = orders.Where(s => s.State != OrderState.Delete && s.State != OrderState.Finish);
            }
            var userids = orders.Select(s => s.ParentUser).ToList();
            userids.AddRange(orders.Select(s => s.User.Recommend).ToList());
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var pids = orders.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            foreach (var item in orders)
            {
                var parent = users.FirstOrDefault(s => s.Id == item.ParentUser);
                var recommend = users.FirstOrDefault(s => s.Id == item.User.Recommend);
                item.ParentUser = parent == null ? "公司" : parent.RealName;
                item.Recommend = recommend == null ? "公司" : recommend.RealName;
                var p = ps.FirstOrDefault(s => s.ID == item.ProductID);
                item.Products = p;
            }
            var paged = orders.AsQueryable().OrderBy(s => s.CreateDateTime).ToPagedList(page);
            if (paged.IsLastPage)
            {
                var foot = orders.GroupBy(s => s.Products).Select(s => new OrderFoot()
                {
                    Product = s.Key,
                    Count = s.Sum(c => c.Count),
                    Send = s.Sum(c => c.Send),
                    Total = s.Sum(c => c.Total),
                    Owe = s.Sum(c => c.Count) - s.Sum(c => c.Send)
                }).ToList();
                ViewBag.Foot = foot;
            }
            return View(paged);
        }

        public ActionResult AllOrder(string userID, string parentUser, OrderViewState? state,
            DateTime? start, DateTime? end, int page = 1)
        {
            Sidebar();
            var orders = GetAllOrder(userID, parentUser, state, start, end);
            var list = orders.AsQueryable()
                .OrderBy(s => s.CreateDateTime)
                .ToPagedList(page);
            if (list.IsLastPage)
            {
                var foot = orders.GroupBy(s => s.Products).Select(s => new OrderFoot()
                {
                    Product = s.Key,
                    Send = s.Sum(c => c.Send),
                    Count = s.Sum(c => c.Count),
                    Total = s.Sum(c => c.Total),
                    Owe = s.Sum(c => c.Count) - s.Sum(c => c.Send),
                });
                ViewBag.Foot = foot.ToList();
            }
            return View(list);
        }

        public ActionResult AllOrderExport(string userID, string parentUser, OrderViewState? state,
            DateTime? start, DateTime? end)
        {
            var list = GetAllOrder(userID, parentUser, state, start, end);
            ViewBag.Stock = GetFoot(list);
            this.AddExcelExportHead($"全部订单{DateTime.Now:yyyyMMdd}");
            return View(list);
        }

        public List<Order> GetAllOrder(string userID, string parentUser, OrderViewState? state,
            DateTime? start, DateTime? end)
        {
            var order = db.Orders.Include(s => s.User).AsQueryable();
            if (!string.IsNullOrWhiteSpace(userID))
            {
                userID = userID.Trim();
                order = order.Where(s => s.User.RealName == userID || s.User.UserName == userID);
            }
            if (state.HasValue)
            {
                switch (state)
                {
                    case OrderViewState.UnPay:
                        order = order.Where(s => s.State == OrderState.UnPay);
                        break;
                    case OrderViewState.Pay:
                        order = order.Where(s => s.State == OrderState.Pay);
                        break;
                    case OrderViewState.Send:
                        order = order.Where(s => s.State == OrderState.Send);
                        break;
                    case OrderViewState.Finish:
                        order = order.Where(s => s.State == OrderState.Finish);
                        break;
                    case OrderViewState.Delete:
                        order = order.Where(s => s.State == OrderState.Delete);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                order = order.Where(s => s.State != OrderState.Delete && s.State != OrderState.Finish);
            }
            if (start.HasValue)
            {
                order = order.Where(s => s.CreateDateTime >= start);
            }
            if (end.HasValue)
            {
                order = order.Where(s => s.CreateDateTime <= end);
            }
            var userids = order.Select(s => s.ParentUser).ToList();
            userids.AddRange(order.Select(s => s.User.Recommend).ToList());
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var pids = order.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            if (!string.IsNullOrWhiteSpace(parentUser))
            {
                if (parentUser == "公司")
                {
                    order = order.Where(s => s.ParentUser == null);
                }
                else
                {
                    var parents = users.FirstOrDefault(s => s.UserName == parentUser);
                    if (parents == null)
                    {
                        order = order.Where(s => s.ParentUser == parentUser);
                    }
                    else
                    {
                        order = order.Where(s => s.ParentUser == parents.Id);
                    }
                }
            }
            foreach (var item in order)
            {
                var parent = users.FirstOrDefault(s => s.Id == item.ParentUser);
                var recommend = users.FirstOrDefault(s => s.Id == item.User.Recommend);
                var p = ps.FirstOrDefault(s => s.ID == item.ProductID);
                item.Products = p;
                item.ParentUser = parent == null ? "公司" : parent.RealName;
                item.Recommend = recommend == null ? "公司" : recommend.RealName;
            }
            return order.ToList();
        }

        public ActionResult Details(int id)
        {
            Sidebar();
            var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.ID == id);
            var p = db.Products.FirstOrDefault(s => s.ID == order.ProductID);
            var model = new OrderViewModel(order, p);
            return View(model);
        }

        // POST: Order/Create
        [HttpPost]
        public ActionResult Create(Order model)
        {
            var userProduct = db.UserProducts.Include(s => s.User)
                .FirstOrDefault(s => s.UserID == UserID && s.ProductID == model.ProductID);
            var orderCount = db.Orders.Count(s => s.UserID == UserID);
            if (model.Count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", $"下单数量不能小于等于0"));
            }
            if (orderCount == 0)
            {
                if (model.Count < userProduct.Min)
                {
                    return Json(Comm.ToMobileResult("Error", $"下单数量不小于{userProduct.Min}"));
                }
            }
            if (orderCount == 1)
            {
                if (model.Count < userProduct.TwiceMin)
                {
                    return Json(Comm.ToMobileResult("Error", $"下单数量不小于{userProduct.TwiceMin}"));
                }
            }
            var order = new Order()
            {
                Code = $"{DateTime.Now.ToString("yyyyMMdd")}{Comm.Random.Next(1000, 9999)}",
                Count = model.Count,
                CreateDateTime = DateTime.Now,
                Send = 0,
                Price = userProduct.Price,
                IsPay = false,
                Total = userProduct.Price * model.Count,
                UserID = UserID,
                ProductID = model.ProductID,
                State = OrderState.UnPay,
                ParentUser = userProduct.User.Parent,
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "下单成功"));
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            Sidebar();
            var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.ID == id);
            order.DeliverHistory = order.DeliverHistory.Where(s => s.CheckState == CheckState.Pass).ToList();
            var p = db.Products.FirstOrDefault(s => s.ID == order.ProductID);
            var model = new OrderViewModel(order, p);
            var sp = db.SupplierProducts
                .Include(s => s.Supplier)
                .Where(s => s.ProductID == order.ProductID)
                .Select(s => s.Supplier).Distinct().ToList();
            if (string.IsNullOrWhiteSpace(model.ParentUser))
            {
                ViewBag.SupplierProduct = sp.Select(s => new SelectListItem()
                {
                    Value = s.ID.ToString(),
                    Text = s.Name
                }).ToList();
                if (!model.ReceiptDateTime.HasValue)
                {
                    ViewBag.Payee = Bll.Account.GetPayeeInSelect();
                }
            }
            else
            {
                var parents = db.UserProducts.FirstOrDefault(s => s.ProductID == order.ProductID && s.UserID == order.ParentUser);
                ViewBag.Count = parents == null ? 0 : parents.Count;
            }
            return View(model);
        }

        // POST: Order/Edit/5
        [HttpPost]
        public ActionResult EditByPay(int id, decimal fee, int payeeID = 0)
        {
            var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.ID == id);
            if (order.ReceiptDateTime.HasValue)
            {
                return Json(Comm.ToMobileResult("Error", "订单已收款"));
            }
            if (string.IsNullOrWhiteSpace(order.ParentUser))
            {
                var mission = db.Missions.Where(s => s.Type == MissionType.Receivables && s.DataID == id &&
                    s.State == Enums.MissionState.CompleteNoCheck);
                if (mission.Count() > 0)
                {
                    return Json(Comm.ToMobileResult("Error", "订单收款已申请审核"));
                }
                if (payeeID == 0)
                {
                    return Json(Comm.ToMobileResult("Error", "请选择收款人"));
                }
                var payee = db.Payees.FirstOrDefault(s => s.ID == payeeID);
                var accoundKind = db.AccountKinds.FirstOrDefault(s => s.Name == "商品类收入");
                var account = new Account()
                {
                    AccountKindID = accoundKind.ID,
                    AllowDelete = false,
                    Amount = order.Total,
                    BankAccount = payee.Name,
                    BankCard = payee.BankCard,
                    BankName = payee.Bank,
                    Fee = fee,
                    IsDelete = false,
                    Remark = $"{order.User.RealName}向公司进货",
                    UserType = order.User.Rank,
                    Trader = order.User.Id,
                    TraderType = TraderType.User,
                    UpdateDateTime = DateTime.Now,
                };
                Receivables r = new Receivables(id, UserID, account);
            }
            else
            {
                order.State = OrderState.Pay;
                order.ReceiptDateTime = DateTime.Now;
                var userProduct = db.UserProducts
                    .FirstOrDefault(s => s.UserID == order.UserID && s.ProductID == order.ProductID);
                userProduct.Sum = userProduct.Sum + order.Count;
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "已确认付款"));
        }

        [HttpPost]
        public ActionResult CancelOrder(int id)
        {
            var order = db.Orders.FirstOrDefault(s => s.ID == id);
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            if (user.Type == Enums.User.Admin)
            {
                if (order.State != OrderState.UnPay && order.State != OrderState.Pay)
                {
                    return Json(Comm.ToMobileResult("Error", "订单不可取消"));
                }
                var userProducts = db.UserProducts.FirstOrDefault(s => s.ProductID == order.ProductID && s.UserID == order.UserID);
                userProducts.Sum = userProducts.Sum - order.Count;
            }
            else
            {
                if (order.ParentUser != UserID || order.State != OrderState.UnPay)
                {
                    return Json(Comm.ToMobileResult("Error", "订单不可取消"));
                }
            }
            order.State = OrderState.Delete;
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "订单已取消"));

        }

        [HttpPost]
        public ActionResult OrderByCode(string code)
        {
            var order = db.Orders.FirstOrDefault(s => s.Code == code);
            if (order == null)
            {
                return Json(Comm.ToMobileResult("Error", "找不到订单"));
            }
            order.Products = db.Products.FirstOrDefault(s => s.ID == order.ProductID);
            return Json(Comm.ToMobileResult("Success", "成功", new
            {
                data = new
                {
                    order.ID,
                    order.Count,
                    order.Products.Name,
                    ProductID = order.Products.ID,
                    order.Total,
                    order.Price
                }
            }));
        }

    }
}
