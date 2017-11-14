using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class UserIncomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "收入管理";
        }

        // GET: UserIncome
        //直推奖明细报表
        public ActionResult Index(bool? isPay, DateTime? start, DateTime? end,
            Enums.UserIncomeType? type, int page = 1)
        {
            Sidebar();
            var list = GetDetails(isPay, start, end, type);
            var paged = list.AsQueryable().ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Amount = new UserIncomeViewModel()
                {
                    Total = list.Sum(s => s.Total),
                    Count = list.Sum(s => s.Count),
                    Amount = list.Sum(s => s.Amount),
                };
            }
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
            return View(paged);
        }

        public ActionResult DetailsExport(bool? isPay, DateTime? start, DateTime? end,
            Enums.UserIncomeType? type, int page = 1)
        {
            var model = GetDetails(isPay, start, end, type).AsQueryable().ToPagedList(page);
            this.AddExcelExportHead($"直推奖明细报表{DateTime.Now:yyyyMMdd}");
            return View(model);
        }

        public List<DirectDetailsViewModel> GetDetails(bool? isPay, DateTime? start, DateTime? end,
            Enums.UserIncomeType? type)
        {
            type = type.HasValue ? type : Enums.UserIncomeType.Bonus;
            var userIncomes = db.UserIncomes.Include(s => s.User)
                .Where(s => s.Type == type);
            if (start.HasValue)
            {
                userIncomes = userIncomes.Where(s => s.CreateDateTime >= start);
            }
            if (end.HasValue)
            {
                userIncomes = userIncomes.Where(s => s.CreateDateTime <= end);
            }
            if (isPay.HasValue)
            {
                userIncomes = userIncomes.Where(s => s.IsPay == isPay);
            }
            userIncomes = userIncomes.OrderBy(s => s.CreateDateTime);
            List<DirectDetailsViewModel> list = new List<DirectDetailsViewModel>();
            var oids = userIncomes.Select(s => s.DateID).Distinct();
            var orders = db.Orders.Include(s => s.User).Where(s => oids.Contains(s.ID)).ToList();
            var userids = orders.Select(s => s.ParentUser).ToList();
            userids.AddRange(orders.Select(s => s.User).Select(s => s.Recommend).ToList());
            userids.Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var pids = orders.Select(s => s.ProductID).Distinct();
            var ps = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            foreach (var item in userIncomes)
            {
                var o = orders.FirstOrDefault(s => s.ID == item.DateID);
                var parent = users.FirstOrDefault(s => s.Id == o.ParentUser);
                parent = parent == null ? new ApplicationUser() : parent;
                var p = ps.FirstOrDefault(s => s.ID == o.ProductID);
                var recommend = users.FirstOrDefault(s => s.Id == o.User.Recommend);
                recommend = recommend == null ? new ApplicationUser() : recommend;
                list.Add(new DirectDetailsViewModel()
                {
                    ID = item.ID,
                    Bonus = item.Amount,
                    Count = o.Count,
                    Pay = item.BankCard,
                    Price = o.Price,
                    IsPay = item.IsPay,
                    ProductName = p.Name,
                    Receivable = recommend.BankCard,
                    ReceivableName = recommend.RealName,
                    ReceivableNumber = recommend.BankCode,
                    ReceivableRank = recommend.Rank,
                    ReceivablesIDCard = recommend.IDCard,
                    Remark = $"{o.User.RealName}进货10%货款奖金",
                    Seller = string.IsNullOrWhiteSpace(parent.RealName) ? "公司" : parent.RealName,
                    Time = item.CreateDateTime,
                    Total = o.Total,
                    Amount = item.Amount
                });
            }
            return list;
        }

        public ActionResult UserIndex(bool? ispay, Enums.UserIncomeType? type, int page = 1)
        {
            Sidebar();
            var userIncome = db.UserIncomes.Include(s => s.User).Where(s => s.UserID == UserID);
            if (type.HasValue)
            {
                userIncome = userIncome.Where(s => s.Type == type);
            }
            if (ispay.HasValue)
            {
                userIncome = userIncome.Where(s => s.IsPay == ispay);
            }
            var list = from ui in userIncome
                       from u in db.Users
                       from o in db.Orders
                       from p in db.Products
                       where ui.RecommendID == u.Id && ui.DateID == o.ID && o.ProductID == p.ID
                       select new { ui, u, o, p };
            var model = new List<UserIncomeViewModel>();
            foreach (var item in list)
            {
                model.Add(new UserIncomeViewModel(item.ui, item.p, item.u, item.o));
            }
            var paged = model.AsQueryable().OrderByDescending(s => s.CreateDateTime).ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Foot = new UserIncomeViewModel()
                {
                    Amount = model.Sum(s => s.Amount),
                    Count = model.Sum(s => s.Count),
                    Total = model.Sum(s => s.Total),
                };
            }
            return View(paged);
        }

        public ActionResult Payment(string ids, int payeeId)
        {
            Sidebar();
            return View(GetPayment(ids, payeeId));
        }

        public ActionResult PaymentExport(string ids, int payeeId)
        {
            var model = GetPayment(ids, payeeId);
            this.AddExcelExportHead($"直推奖付款报表{DateTime.Now:yyyyMMdd}");
            return View(model);
        }

        public List<DirectPaymentViewModel> GetPayment(string ids, int payeeId)
        {
            var list = new List<DirectPaymentViewModel>();
            var idList = ids.SplitToIntArray();
            if (idList == null)
            {
                return list;
            }
            var userIncomes = db.UserIncomes.Include(s => s.User).Where(s => idList.Contains(s.ID)).ToList();
            var oids = userIncomes.Select(s => s.DateID).Distinct();
            var orders = db.Orders.Include(s => s.User).Where(s => oids.Contains(s.ID));
            var payee = db.Payees.FirstOrDefault(s => s.ID == payeeId);
            foreach (var item in userIncomes)
            {
                var o = orders.FirstOrDefault(s => s.ID == item.DateID);
                list.Add(new DirectPaymentViewModel()
                {
                    ID = item.ID,
                    Pay = payee.BankCard,
                    ReceivableName = item.User.RealName,
                    ReceivableNumber = item.User.BankCode,
                    Remark = $"{o.User.RealName}进货10%货款奖金",
                    Totla = item.Amount,
                    ReceivableBankName = item.User.Bank,
                    ReceivableCard = item.User.BankCard,
                    OrderUser = o.User.RealName
                });
            }
            var payList = list.GroupBy(s => new { s.ReceivableCard, s.ReceivableBankName, s.ReceivableName, s.ReceivableNumber })
                .Select(q => new DirectPaymentViewModel()
                {
                    Pay = payee.BankCard,
                    ReceivableNumber = q.Key.ReceivableNumber,
                    ReceivableName = q.Key.ReceivableName,
                    ReceivableBankName = q.Key.ReceivableBankName,
                    ReceivableCard = q.Key.ReceivableCard,
                    Remark = $"{string.Join(",", q.Select(w => w.OrderUser).Distinct())}进货10%货款奖金",
                    Totla = q.Sum(t => t.Totla),
                }).ToList();
            return payList;
        }

        [HttpPost]
        public ActionResult EditIsPay(string ids, int payee)
        {
            var idList = ids.SplitToIntArray();
            if (idList == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有要发的直推奖"));
            }
            var userIncomes = db.UserIncomes.Include(s => s.User).Where(s => idList.Contains(s.ID)).ToList();
            var orderids = userIncomes.Select(s => s.DateID).Distinct().ToList();
            var orders = db.Orders.Include(s => s.User).Where(s => orderids.Contains(s.ID));
            var payees = db.Payees.FirstOrDefault(s => s.ID == payee);
            var list = Bll.Account.GetPayeeAccount(payees.Bank, payees.BankCard);
            if (userIncomes.Sum(s => s.Amount) > list)
            {
                return Json(Comm.ToMobileResult("Error", $"付款账户金额不足,账户剩余{list}元"));
            }
            if (userIncomes.Any(s => s.IsPay == true))
            {
                return Json(Comm.ToMobileResult("Error", $"个别直推奖已经发出"));
            }
            foreach (var item in userIncomes)
            {
                item.IsPay = true;
                item.ReceiptDateTime = DateTime.Now;
                item.BankAccount = payees.Name;
                item.BankCard = payees.BankCard;
                item.BankName = payees.Bank;
                var order = orders.FirstOrDefault(s => s.ID == item.DateID);
                order.IsPay = true;
                try
                {
                    var account = new Account()
                    {
                        Amount = item.Amount,
                        Fee = 0,
                        Remark = $"{item.User.RealName}直推{order.User.RealName}奖金，将{order.User.RealName}进货10%货款奖励给{item.User.RealName}",
                        Trader = item.User.Id,
                        TraderType = Enums.TraderType.User,
                        UserType = item.User.Rank,
                        UpdateDateTime = DateTime.Now,
                    };
                    Bll.Account.Create(account, "直推奖", payee);
                }
                catch (Exception ex)
                {
                    return Json(Comm.ToMobileResult("Error", ex.Message));
                }
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "已打款"));
        }

    }
}
