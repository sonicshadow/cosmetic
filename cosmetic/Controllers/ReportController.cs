using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using System.Data.Entity;
using PagedList;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "报表管理";
        }

        [Authorize(Roles = SysRole.ReportRead)]
        public ActionResult Index()
        {
            Sidebar();
            return View();
        }

        //库存报表
        private IQueryable<NowStockViewModel> GetAllUserStore(string filter = null, Enums.UserType? rank = null)
        {
            Enums.UserType? temp = null;
            var cQuery = from s in (
                          from s in db.SupplierProducts
                          group s by s.ProductID into sg
                          select new
                          {
                              ProductID = sg.Key,
                              Count = sg.Sum(s => s.Remaining)
                          })
                         from p in db.Products
                         where s.ProductID == p.ID
                         join pd in db.ProductDetails
                             .Where(x => x.UseType == Enums.UserType.Premium)
                             on p.ID equals pd.ProductID into gpd
                         from pdu in gpd.DefaultIfEmpty()
                         select new NowStockViewModel
                         {
                             ProductName = p.Name,
                             Count = s.Count,
                             OwnerName = "公司",
                             Price = pdu.Price,
                             Rank = temp,
                             Recommend = "",
                             UserName = "",
                             IDCard = ""
                         };
            var query = (from up in db.UserProducts
                         from p in db.Products
                         from u in db.Users
                         where up.ProductID == p.ID && u.Id == up.UserID && up.Count != 0
                         join g in db.Users on up.User.Recommend equals g.Id into ur
                         from c in ur.DefaultIfEmpty()
                         select new NowStockViewModel
                         {
                             ProductName = p.Name,
                             Count = up.Count,
                             OwnerName = u.RealName,
                             Price = up.Price,
                             Rank = (Enums.UserType?)u.Rank,
                             Recommend = c.RealName,
                             UserName = u.UserName,
                             IDCard = u.IDCard
                         });
            var list = cQuery.Union(query);
            if (!string.IsNullOrWhiteSpace(filter))
            {
                list = list.Where(s => s.UserName == filter);
            }
            if (rank.HasValue)
            {
                list = list.Where(s => s.Rank == rank);
            }
            return list;
        }

        public ActionResult AllUserStore(int page = 1, string filter = null, Enums.UserType? rank = null)
        {
            Sidebar();
            var paged = GetAllUserStore(filter, rank).OrderBy(s => s.Rank).ThenBy(s => s.IDCard).ToPagedList(page);
            return View(paged);
        }

        public ActionResult AllUserStoreExport(int page = 1, string filter = null, Enums.UserType? rank = null)
        {
            var cQuery = GetAllUserStore(filter, rank).OrderBy(s => s.Rank).ThenBy(s => s.IDCard).ToPagedList(page);
            this.AddExcelExportHead($"当前所有用户库存报表{DateTime.Now:yyyyMMdd}");
            return View(cQuery);
        }

        //销售报表
        public ActionResult Sale(string name, DateTime? start, DateTime? end, int page = 1)
        {
            Sidebar();
            var model = GetSale(name, start, end, page).OrderBy(s => s.Time).ToPagedList(page, 15);
            return View(model);
        }

        public ActionResult SaleExport(string name, DateTime? start, DateTime? end, int page = 1)
        {
            var model = GetSale(name, start, end, page).OrderBy(s => s.Time).ToPagedList(page, 15);
            this.AddExcelExportHead($"销售报表{DateTime.Now:yyyyMMdd}");
            return View(model);
        }

        public List<SaleViewModel> GetSale(string name, DateTime? start, DateTime? end, int page = 1)
        {
            var user = new ApplicationUser();
            List<SaleViewModel> model = new List<SaleViewModel>();
            if (!string.IsNullOrWhiteSpace(name))
            {
                user = db.Users.FirstOrDefault(s => s.PhoneNumber == name);
                if (user == null)
                {
                    return model;
                }
                name = user.Id;
            }
            name = string.IsNullOrWhiteSpace(name) ? null : name;
            var order = db.Orders.Include(s => s.User).Where(s => s.ParentUser == name);
            if (start.HasValue)
            {
                order = order.Where(s => s.CreateDateTime >= start.Value);
            }
            if (end.HasValue)
            {
                order = order.Where(s => s.CreateDateTime <= end.Value);
            }
            var pids = order.Select(s => s.ProductID).Distinct().ToList();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            var buyingPrices = new List<UserProduct>();
            if (string.IsNullOrWhiteSpace(name))
            {
                buyingPrices = db.SupplierProducts
                     .Where(s => pids.Contains(s.ProductID)).ToList()
                     .Select(s => new UserProduct()
                     {
                         ProductID = s.ProductID,
                         Price = s.Price
                     }).ToList();
            }
            else
            {
                buyingPrices = db.UserProducts.Where(s => s.UserID == name).ToList();
            }
            var list = order.OrderBy(s => s.CreateDateTime).ToPagedList(page);
            foreach (var item in order)
            {
                var p = ps.FirstOrDefault(s => s.ID == item.ProductID);
                var buyingPrice = buyingPrices.FirstOrDefault(s => s.ProductID == item.ProductID);
                buyingPrice = buyingPrice == null ? new UserProduct() : buyingPrice;
                model.Add(new SaleViewModel()
                {
                    Count = item.Count,
                    Price = item.Price,
                    ProductName = p.Name,
                    Seller = string.IsNullOrWhiteSpace(user.RealName) ? "公司" : user.RealName,
                    SalesProfit = (item.Price - buyingPrice.Price) * item.Count,
                    BuyingPrice = buyingPrice.Price,
                    Difference = item.Price - buyingPrice.Price,
                    Remark = "",
                    Time = item.CreateDateTime,
                    Total = item.Total,
                    UserName = item.User.RealName,
                    UserRank = item.User.Rank,
                });
            }
            return model;
        }


        //进货报表
        public ActionResult Storage(string to, string from, DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(GetStorage(to, from, start, end));
        }

        public ActionResult StorageExport(string to, string from, DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"进货报表{DateTime.Now:yyyyMMdd}");
            return View(GetStorage(to, from, start, end));
        }

        public List<StorageViewModel> GetStorage(string to, string from, DateTime? start, DateTime? end)
        {
            var list = new List<StorageViewModel>();
            var stocks = db.Stock.Include(s => s.Product)
                .Where(s => s.Type == Enums.StockType.AddStock || s.Type == Enums.StockType.Deliver);
            if (!string.IsNullOrWhiteSpace(to) && to != "公司")
            {
                //会员的全部进货信息
                var users = db.Users.Where(s => s.UserName == to || s.UserName == from);
                var toUser = users.FirstOrDefault(s => s.UserName == to);
                if (toUser == null)
                {
                    return list;
                }
                stocks = stocks.Where(s => s.UserID == toUser.Id);
                var dataids = stocks.Select(s => s.DataID).Distinct();
                var orders = db.Orders.Include(s => s.User).Where(s => dataids.Contains(s.ID)).ToList();
                //会员进货
                if (!string.IsNullOrWhiteSpace(from))
                {
                    if (from == "公司")
                    {
                        orders = orders.Where(s => s.ParentUser == null).ToList();
                    }
                    else
                    {
                        //公司或会员 进货的信息
                        var fromUser = users.FirstOrDefault(s => s.UserName == from);
                        if (fromUser == null)
                        {
                            return list;
                        }
                        orders = orders.Where(s => s.ParentUser == fromUser.Id).ToList();
                    }
                }
                var parentids = orders.Select(s => s.ParentUser).Distinct();
                var parentUsers = db.Users.Where(s => parentids.Contains(s.Id)).ToList();
                foreach (var item in stocks)
                {
                    var order = orders.FirstOrDefault(s => s.ID == item.DataID);
                    var parent = parentUsers.FirstOrDefault(s => s.Id == order.ParentUser);
                    var supplierName = string.IsNullOrWhiteSpace(order.ParentUser) ? "公司" : parent.RealName;
                    Enums.UserType? parentRank = null;
                    parentRank = string.IsNullOrWhiteSpace(order.ParentUser) ? parentRank : parent.Rank;
                    list.Add(new StorageViewModel()
                    {
                        Code = order.Code,
                        Count = -item.Count,
                        ID = item.ID,
                        Name = order.User.RealName,
                        ParentRank = parentRank,
                        Price = order.Price,
                        ProductName = item.Product.Name,
                        Rank = order.User.Rank,
                        Remark = item.Remark,
                        SupplierName = supplierName,
                        Time = item.CreateTime,
                        Total = order.Price * -item.Count
                    });
                }
            }
            else
            {
                //查全部进货
                stocks = stocks.Where(s => s.UserID == null);
                var dataids = stocks.Select(s => s.DataID).Distinct();
                var sps = db.SupplierProducts.Include(s => s.Supplier).Where(s => dataids.Contains(s.ID)).ToList();
                //公司进货
                if (!string.IsNullOrWhiteSpace(from))
                {
                    //查个别厂商进货
                    var supplierFrom = sps.Where(s => s.Supplier.Name.Contains(from));
                    if (supplierFrom != null)
                    {
                        var sfids = supplierFrom.Select(s => s.ID);
                        stocks = stocks.Where(s => sfids.Contains(s.DataID));
                    }
                }
                foreach (var item in stocks)
                {
                    var sp = sps.FirstOrDefault(s => s.ID == item.DataID);
                    list.Add(new StorageViewModel()
                    {
                        Code = sp.Code,
                        Count = item.Count,
                        ID = item.ID,
                        Name = "公司",
                        ParentRank = null,
                        Price = sp.Price,
                        ProductName = item.Product.Name,
                        Rank = null,
                        Remark = item.Remark,
                        SupplierName = sp.Supplier.Name,
                        Time = item.CreateTime,
                        Total = sp.Price * item.Count
                    });
                }
            }
            return list;
        }


        //未发货订单明细
        public ActionResult SendOrder(string to, string from, DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(GetSendOrder(to, from, start, end));
        }

        public ActionResult SendOrderExport(string to, string from, DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"未发货订单明细{DateTime.Now:yyyyMMdd}");
            return View(GetSendOrder(to, from, start, end));
        }

        public List<SendOrderViewModel> GetSendOrder(string to, string from, DateTime? start, DateTime? end)
        {
            List<SendOrderViewModel> list = new List<SendOrderViewModel>();
            var orders = db.Orders.Include(s => s.User)
                .Where(s => s.State == Enums.OrderState.Pay || s.State == Enums.OrderState.Send);
            if (start.HasValue)
            {
                orders = orders.Where(s => s.CreateDateTime >= start);
            }
            if (end.HasValue)
            {
                orders = orders.Where(s => s.CreateDateTime <= end);
            }
            if (!string.IsNullOrWhiteSpace(to))
            {
                orders = orders.Where(s => s.User.UserName == to);
            }
            var userids = orders.ToList().Select(s => s.ParentUser).Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            if (!string.IsNullOrWhiteSpace(from))
            {
                if (from == "公司")
                {
                    orders = orders.Where(s => s.ParentUser == null);
                }
                else
                {
                    var u = users.FirstOrDefault(s => s.UserName == from);
                    if (u == null)
                    {
                        return list;
                    }
                    orders = orders.Where(s => s.ParentUser == u.Id);
                }
            }
            var pids = orders.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            foreach (var item in orders)
            {
                var product = ps.FirstOrDefault(s => s.ID == item.ProductID);
                Enums.UserType? prank = null;
                if (!string.IsNullOrWhiteSpace(item.ParentUser))
                {
                    var p = users.FirstOrDefault(s => s.Id == item.ParentUser);
                    item.ParentUser = p.RealName;
                    prank = p.Rank;
                }
                else
                {
                    item.ParentUser = "公司";
                }
                list.Add(new SendOrderViewModel()
                {
                    ID = item.ID,
                    Code = item.Code,
                    Count = item.Count,
                    IsSend = item.Count - item.Send,
                    IsSendTotal = (item.Count - item.Send) * item.Price,
                    Parent = item.ParentUser,
                    ParentRank = prank,
                    Price = item.Price,
                    ProductName = product.Name,
                    Remark = "",
                    Send = item.Send,
                    SendTotal = item.Send * item.Price,
                    Time = item.CreateDateTime,
                    Total = item.Total,
                    User = item.User.RealName,
                    UserRank = item.User.Rank,
                });
            }
            return list;
        }

        //销售利润报表
        public ActionResult SalesProfit(string name, DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(GetSalesProfit(name, start, end));
        }

        public ActionResult SalesProfitExport(string name, DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"销售利润报表{DateTime.Now:yyyyMMdd}");
            return View(GetSalesProfit(name, start, end));
        }

        public List<SalesProfitViewModel> GetSalesProfit(string name, DateTime? start, DateTime? end)
        {
            List<SalesProfitViewModel> list = new List<SalesProfitViewModel>();
            list.AddRange(GetOrderProfit(name, start, end));
            list.AddRange(GetIncomeProfit(name, start, end));
            list = list.OrderBy(s => s.Time).ToList();
            return list;
        }

        /// <summary>
        /// 订单利润
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<SalesProfitViewModel> GetOrderProfit(string name, DateTime? start, DateTime? end)
        {
            List<SalesProfitViewModel> list = new List<SalesProfitViewModel>();
            var isCompany = string.IsNullOrWhiteSpace(name) || name == "公司";
            //订单利润
            var orders = db.Orders.Include(s => s.User)
                .Where(s => s.State != Enums.OrderState.Delete && s.State != Enums.OrderState.UnPay);
            //产品名称s
            var pids = orders.Select(s => s.ProductID).Distinct();
            var products = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            //查询父信息
            var userids = orders.Select(s => s.ParentUser).Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            //时间段
            if (start.HasValue)
            {
                orders = orders.Where(s => s.CreateDateTime >= start.Value);
            }
            if (end.HasValue)
            {
                orders = orders.Where(s => s.CreateDateTime <= end.Value);
            }
            //订单销售者
            ApplicationUser u = null;
            if (isCompany)
            {
                orders = orders.Where(s => s.ParentUser == null);
            }
            else
            {
                u = users.FirstOrDefault(s => s.UserName == name);
                if (u == null)
                {
                    return list;
                }
                orders = orders.Where(s => s.ParentUser == u.Id);
            }
            //采购价s
            var buyingPrices = new List<UserProduct>();
            if (u == null)
            {
                buyingPrices = db.SupplierProducts
                     .Where(s => pids.Contains(s.ProductID)).ToList()
                     .GroupBy(s => new { s.ProductID, s.Price })
                     .Select(s => new UserProduct()
                     {
                         ProductID = s.Key.ProductID,
                         Price = s.Key.Price
                     }).ToList();
            }
            else
            {
                buyingPrices = db.UserProducts.Where(s => pids.Contains(s.ProductID) && s.UserID == u.Id).ToList();
            }
            foreach (var item in orders)
            {
                var p = products.FirstOrDefault(s => s.ID == item.ProductID);
                var buyPrice = buyingPrices.FirstOrDefault(s => s.ProductID == item.ProductID);
                var user = users.FirstOrDefault(s => s.Id == item.ParentUser);
                list.Add(new SalesProfitViewModel()
                {
                    BuyingPrice = buyPrice == null ? 0 : buyPrice.Price,
                    Count = item.Count,
                    Difference = item.Price - (buyPrice == null ? 0 : buyPrice.Price),
                    ID = item.ID,
                    IsOrder = true,
                    Parent = user == null ? "公司" : user.RealName,
                    Price = item.Price,
                    ProductName = p.Name,
                    Profit = (item.Price - (buyPrice == null ? 0 : buyPrice.Price)) * item.Count,
                    Remark = "",
                    Time = item.CreateDateTime,
                    Total = item.Total,
                    User = item.User.RealName,
                    UserRank = item.User.Rank,
                });
            }
            return list;
        }

        /// <summary>
        /// 奖励利润
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public List<SalesProfitViewModel> GetIncomeProfit(string name, DateTime? start, DateTime? end)
        {
            List<SalesProfitViewModel> list = new List<SalesProfitViewModel>();
            var isCompany = string.IsNullOrWhiteSpace(name) || name == "公司";
            var userincomes = db.UserIncomes.Include(s => s.User)
                .Where(ui => ui.IsPay == true && ui.Type == Enums.UserIncomeType.Bonus);
            if (start.HasValue)
            {
                userincomes = userincomes.Where(s => s.CreateDateTime >= start);
            }
            if (end.HasValue)
            {
                userincomes = userincomes.Where(s => s.CreateDateTime <= end);
            }
            var oids = userincomes.Select(s => s.DateID).Distinct();
            var os = db.Orders.Include(s => s.User).Where(s => oids.Contains(s.ID)).ToList();
            var userids = userincomes.Select(s => s.RecommendID).ToList();
            userids.AddRange(os.Select(s => s.ParentUser).ToList());
            userids.AddRange(os.Select(s => s.User).Select(s => s.Recommend));
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var pids = os.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            var recommend = users.FirstOrDefault(s => s.UserName == name);
            if (!isCompany)
            {
                if (recommend == null)
                {
                    return list;
                }
                userincomes = userincomes.Where(s => s.UserID == recommend.Id);
            }
            foreach (var item in userincomes)
            {
                var order = os.FirstOrDefault(s => s.ID == item.DateID);
                var p = ps.FirstOrDefault(s => s.ID == order.ProductID);
                var user = users.FirstOrDefault(s => s.Id == order.ParentUser);
                var re = users.FirstOrDefault(s => s.Id == order.User.Recommend);
                list.Add(new SalesProfitViewModel()
                {
                    BuyingPrice = 0,
                    Count = order.Count,
                    Difference = isCompany ? -order.Price : order.Price,
                    ID = order.ID,
                    IsOrder = false,
                    Parent = user == null ? "公司" : user.RealName,
                    Price = order.Price,
                    ProductName = p.Name,
                    Profit = isCompany ? -item.Amount : item.Amount,
                    Remark = $"{re.RealName}直推{order.User.RealName}奖金，将{order.User.RealName}进货10%货款奖励给{re.RealName}",
                    Time = order.CreateDateTime,
                    Total = order.Total,
                    User = order.User.RealName,
                    UserRank = order.User.Rank,
                });
            }
            return list;
        }

        //公司利润报表
        public ActionResult Profit(DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(GetProfit(start, end));
        }

        public ActionResult ProfitExport(DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"公司利润报表{DateTime.Now:yyyyMMdd}");
            return View(GetProfit(start, end));
        }

        public List<ProfitViewModel> GetProfit(DateTime? start, DateTime? end)
        {
            //公司利润报表的逻辑错了，简单一点就是用销售利润报表的结存再减掉直推奖、
            //减掉其它费用支出、再减掉股东分红、加上其它业务收入（其它的类目都不要算进去）
            var salesProfit = GetSalesProfit(null, start, end).Sum(s => s.Profit);
            var accounts = db.Accounts.Where(s => !s.IsDelete &&
                (s.AccountKind.Name == "其它费用支出" ||
                s.AccountKind.Name == "股东分红" ||
                s.AccountKind.Name == "其它业务收入"));
            if (start.HasValue)
            {
                accounts = accounts.Where(s => s.UpdateDateTime >= start);
            }
            if (end.HasValue)
            {
                accounts = accounts.Where(s => s.UpdateDateTime <= end);
            }
            var a = new List<ProfitViewModel>();
            //销售利润报表的结存
            a.Add(new ProfitViewModel()
            {
                AccountKind = "销售利润",
                Remark = "销售利润报表里面的利润合计",
                Total = salesProfit,
            });
            a.AddRange(accounts.GroupBy(s => s.AccountKind)
               .Select(s => new ProfitViewModel()
               {
                   AccountKind = s.Key.Name,
                   Remark = "",
                   Total = (s.Key.Type != Enums.AccountKindType.Pay ? s.Sum(q => q.Amount) : -s.Sum(q => q.Amount)) - s.Sum(q => q.Fee)
               }).ToList());
            foreach (var item in a)
            {
                item.ID = a.IndexOf(item) + 1;
            }
            return a;
        }

        //帐户资金进出存报表
        public ActionResult Account(string trader, string bankName, string bandCard, DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(Bll.Account.GetAccount(trader, bankName, bandCard, start, end));
        }

        public ActionResult AccountExport(string trader, string bankName, string bandCard, DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"帐户资金进出结存报表{DateTime.Now:yyyyMMdd}");
            return View(Bll.Account.GetAccount(trader, bankName, bandCard, start, end));
        }

        public ActionResult BankCapital(DateTime? start, DateTime? end)
        {
            Sidebar();
            return View(GetBankCapital(start, end));
        }
        public ActionResult BankCapitalExport(DateTime? start, DateTime? end)
        {
            this.AddExcelExportHead($"银行资金总额分布报表{DateTime.Now:yyyyMMdd}");
            return View(GetBankCapital(start, end));
        }
        public List<BankCapital> GetBankCapital(DateTime? start, DateTime? end)
        {
            var list = Bll.Account.GetAccount(null, null, null, start, end);
            var model = list.GroupBy(s => new { s.BankName, s.BankCard }).Select(s => new BankCapital()
            {
                BankName = s.Key.BankName,
                BankAccount = s.Key.BankCard,
                Total = s.Where(w => !w.IsDelete).Sum(q => q.Total),
            }).ToList();
            return model;
        }

        //股东分红报表
        public ActionResult ShareHolder(decimal total)
        {
            Sidebar();
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
            return View(GetShareHolder(total));
        }

        public ActionResult ShareHolderExport(decimal total)
        {
            this.AddExcelExportHead($"公司股东分红报表{DateTime.Now:yyyyMMdd}");
            return View(GetShareHolder(total));
        }

        public List<ShareHolder> GetShareHolder(decimal total)
        {
            var holders = db.Holders.ToList();
            var average = total / holders.Sum(s => s.Stock);
            var list = new List<ShareHolder>();
            foreach (var item in holders)
            {
                list.Add(new ShareHolder()
                {
                    ID = holders.ToList().IndexOf(item) + 1,
                    Realname = item.UserID,
                    IDCard = item.IDCard,
                    BankCard = item.BankCard,
                    BankName = item.BankName,
                    Stock = item.Stock,
                    Total = Math.Round((item.Stock * average), 2),
                });
            }
            return list;
        }

        public ActionResult Share(DateTime? start, DateTime? end)
        {
            Sidebar();
            var list = GetProfit(start, end).Sum(s => s.Total);
            ViewBag.Profit = list;
            return View();
        }

        [HttpPost]
        public ActionResult Transfer(decimal total, int payee, DateTime time)
        {
            var list = GetShareHolder(total);
            foreach (var item in list)
            {
                try
                {
                    var account = new Account()
                    {
                        Fee = 0,
                        Amount = item.Total,
                        Trader = item.Realname,
                        TraderType = Enums.TraderType.Other,
                        Remark = "股东分红",
                        UpdateDateTime = time,
                    };
                    Bll.Account.Create(account, "股东分红", payee);
                }
                catch (Exception ex)
                {
                    return Json(Comm.ToMobileResult("Error", ex.Message));
                }
            }
            return Json(Comm.ToMobileResult("Success", "转账成功"));
        }
    }
}