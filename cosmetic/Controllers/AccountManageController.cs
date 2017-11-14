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
using Newtonsoft.Json;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class AccountManageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {
            ViewBag.Sidebar = "资金记录";
            return;
        }

        [Authorize(Roles = SysRole.AccountManageRead)]
        public ActionResult Index(string bandCard, int page = 1, int? kind = null,
            DateTime? start = null, DateTime? end = null)
        {
            Sidebar();
            ViewBag.AllKind = db.AccountKinds.ToList();
            var accounts = db.Accounts.Include(a => a.AccountKind).OrderBy(s => s.UpdateDateTime).AsQueryable();
            decimal total = 0;
            foreach (var item in accounts)
            {
                bool fu = item.AccountKind.Type != Enums.AccountKindType.Pay;
                total = item.IsDelete ? total : total + (fu ? item.Amount : -item.Amount) - item.Fee;
                item.Amount = fu ? item.Amount : -item.Amount;
                item.Totla = total;
            }
            if (!string.IsNullOrWhiteSpace(bandCard))
            {
                accounts = accounts.Where(s => s.BankCard == bandCard);
            }
            if (kind.HasValue)
            {
                accounts = accounts.Where(s => s.AccountKindID == kind);
            }
            if (start.HasValue)
            {
                accounts = accounts.Where(s => s.UpdateDateTime >= start);
            }
            if (end.HasValue)
            {
                accounts = accounts.Where(s => s.UpdateDateTime <= end);
            }
            var paged = accounts.OrderByDescending(s => s.UpdateDateTime).ToPagedList(page);
            return View(paged);
        }

        [Authorize(Roles = SysRole.AccountManageRead)]
        public ActionResult Details(int? id)
        {
            Sidebar();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        [Authorize(Roles = SysRole.AccountManageCreate)]
        public ActionResult Create()
        {
            GetView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountManageCreate)]
        public ActionResult Create(Account account, int PayeeID)
        {
            var payee = db.Payees.FirstOrDefault(s => s.ID == PayeeID);
            if (ModelState.IsValid || payee == null)
            {
                account.BankAccount = payee.Name;
                account.BankCard = payee.BankCard;
                account.BankName = payee.Bank;
                account.AllowDelete = true;
                account.IsDelete = false;
                account.TraderType = Enums.TraderType.Other;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            GetView();
            return View(account);
        }

        public void GetView()
        {
            Sidebar();
            ViewBag.AccountKindID = new SelectList(db.AccountKinds, "ID", "Name");
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
        }

        [Authorize(Roles = SysRole.AccountManageEdit)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            Sidebar();
            ViewBag.AccountKindID = new SelectList(db.AccountKinds, "ID", "Name", account.AccountKindID);
            var payee = new List<SelectListItem>();
            payee.Add(new SelectListItem()
            {
                Text = $"{account.BankAccount}({account.BankCard} {account.BankName})",
            });
            payee.AddRange(db.Payees.ToList()
              .Select(s => new SelectListItem()
              {
                  Text = $"{s.Name}({s.BankCard} {s.Bank})",
                  Value = s.ID.ToString()
              }).ToList());
            ViewBag.Payee = payee;
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountManageEdit)]
        public ActionResult Edit(Account account, int? PayeeID)
        {
            var payee = db.Payees.FirstOrDefault(s => s.ID == PayeeID);
            if (ModelState.IsValid)
            {
                account.BankAccount = payee.Name;
                account.BankCard = payee.BankCard;
                account.BankName = payee.Bank;
                account.TraderType = Enums.TraderType.Other;
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Sidebar();
            ViewBag.AccountKindID = new SelectList(db.AccountKinds, "ID", "Name", account.AccountKindID);
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
            return View(account);
        }

        [Authorize(Roles = SysRole.AccountManageDelete)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);

            if (account == null)
            {
                return HttpNotFound();
            }
            Sidebar();
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountManageDelete)]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = SysRole.AccountManageCreate)]
        public ActionResult Transfer()
        {
            GetView();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = SysRole.AccountManageCreate)]
        public ActionResult Transfer(DateTime time, int inPayeeId, int outPayeeId, decimal price, int fee)
        {
            GetView();
            var model = new
            {
                time = time,
                inPayeeId = inPayeeId,
                outPayeeId = outPayeeId,
                price = price,
                fee = fee
            };
            if (price <= 0)
            {
                ModelState.AddModelError("price", "填写正确的价格");
                return View(model);
            }
            if (inPayeeId == outPayeeId)
            {
                ModelState.AddModelError("inPayeeId", "转入账号和转出账号一致");
                return View(model);
            }
            var accountKinds = db.AccountKinds.Where(s => s.Name == "内部银行帐户互转");
            var payees = db.Payees.Where(s => s.ID == inPayeeId || s.ID == outPayeeId);
            var inpayee = payees.FirstOrDefault(s => s.ID == inPayeeId);
            if (inpayee == null)
            {
                ModelState.AddModelError("inPayeeId", "请选择转入账号");
                return View(model);
            }
            var inAccountKind = accountKinds.FirstOrDefault(s => s.Type == Enums.AccountKindType.Assets);
            if (inAccountKind == null)
            {
                ModelState.AddModelError("", "请先添加 资产类 内部银行相互转帐 的科目");
                return View(model);
            }
            var inAccount = new Account()
            {
                Fee = 0,
                Remark = "公司账号之间转账",
                TraderType = Enums.TraderType.Payee,
                Trader = inPayeeId.ToString(),
                Amount = price,
                UpdateDateTime = time,
                AccountKindID = inAccountKind.ID,
                AllowDelete = false,
                BankAccount = inpayee.Name,
                BankCard = inpayee.BankCard,
                BankName = inpayee.Bank,
                IsDelete = false,
            };
            var outpayee = payees.FirstOrDefault(s => s.ID == outPayeeId);
            if (outpayee == null)
            {
                ModelState.AddModelError("outPayeeId", "请选择转出账号");
                return View(model);
            }
            var list = Bll.Account.GetPayeeAccount(outpayee.Bank, outpayee.BankCard);
            if (price > list)
            {
                ModelState.AddModelError("outPayeeId", "转出账号余额不足");
                return View(model);
            }
            var outAccountKind = accountKinds.FirstOrDefault(s => s.Type == Enums.AccountKindType.Pay);
            if (outAccountKind == null)
            {
                ModelState.AddModelError("", "请先添加 支出类 内部银行相互转帐 的科目");
                return View(model);
            }
            var outAccount = new Account()
            {
                Fee = fee,
                Remark = "公司账号之间转账",
                TraderType = Enums.TraderType.Payee,
                Trader = outPayeeId.ToString(),
                Amount = price,
                UpdateDateTime = time,
                AccountKindID = outAccountKind.ID,
                AllowDelete = false,
                BankAccount = outpayee.Name,
                BankCard = outpayee.BankCard,
                BankName = outpayee.Bank,
                IsDelete = false,
            };
            db.Accounts.Add(inAccount);
            db.Accounts.Add(outAccount);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UserIndex(int page = 1)
        {
            Sidebar();
            //如A:查询A的订单，A是发货人的订单，以及直推奖的信息
            var orders = db.Orders.Include(s => s.User)
                .Where(s => (s.UserID == UserID || s.ParentUser == UserID) &&
                    s.ReceiptDateTime.HasValue && s.State != Enums.OrderState.Delete).ToList();
            var list = new List<Account>();
            foreach (var item in orders)
            {
                list.Add(new Account()
                {
                    Amount = item.UserID == UserID ? -item.Total : item.Total,
                    UpdateDateTime = item.ReceiptDateTime.Value,
                    Remark = item.UserID == UserID ? "购买商品" : $"出售商品给{item.User.RealName}",
                });
            }
            var userincomes = db.UserIncomes.Where(s => s.UserID == UserID && s.IsPay == true);
            foreach (var item in userincomes)
            {
                list.Add(new Account()
                {
                    Amount = item.Amount,
                    UpdateDateTime = item.ReceiptDateTime.Value,
                    Remark = item.Type == Enums.UserIncomeType.Bonus ? "直推奖奖励" : "升级奖励",
                });
            }
            decimal total = 0;
            list.OrderBy(s => s.UpdateDateTime);
            foreach (var item in list)
            {
                total = total + item.Amount;
                item.Totla = total;
            }
            var paged = list.AsQueryable().ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Amount = list.Sum(s => s.Amount);
            }
            return View(paged);
        }

        [HttpPost]
        public ActionResult Cancel(int id, string remark)
        {
            var account = db.Accounts.FirstOrDefault(s => s.ID == id);
            if (!account.AllowDelete)
            {
                return Json(Comm.ToMobileResult("Error", "记录不可删除"));
            }
            if (account.IsDelete)
            {
                return Json(Comm.ToMobileResult("Error", "记录已经取消了"));
            }
            account.IsDelete = true;
            var modify = new Modify()
            {
                ModifyType = Enums.ModifyType.Account,
                Time = DateTime.Now,
                OldData = id.ToString(),
                NewData = remark,
                UserID = UserID,
            };
            db.Modifys.Add(modify);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }

        public ActionResult CancelModify(int paged = 1)
        {
            Sidebar();
            var modify = db.Modifys.Include(s => s.User).Where(s => s.ModifyType == Enums.ModifyType.Account);
            var ids = modify.Select(s => s.OldData);
            var accounts = db.Accounts.Where(s => ids.Contains(s.ID.ToString())).ToList();
            var list = new List<AccountModify>();
            foreach (var item in modify)
            {
                var account = accounts.FirstOrDefault(s => s.ID.ToString() == item.OldData);
                list.Add(new AccountModify()
                {
                    Account = account,
                    Time = item.Time,
                    Remark = item.NewData,
                    User = item.User,
                    UserID = item.UserID,
                });
            }
            var page = list.AsQueryable().OrderBy(s => s.Time).ToPagedList(paged);
            return View(page);
        }

        public ActionResult Check(Enums.CheckState? check, int page = 1)
        {
            var missions = db.Missions.Include(s => s.User).Where(s => s.Type == Enums.MissionType.Receivables);           
            ViewBag.NoCheckCount = missions.ToList().Count(s => s.State == Enums.MissionState.CompleteNoCheck);
            if (check.HasValue)
            {
                switch (check)
                {
                    case Enums.CheckState.Pass:
                        missions = missions.Where(s => s.State == Enums.MissionState.CompletePass);
                        break;
                    case Enums.CheckState.NoPass:
                        missions = missions.Where(s => s.State == Enums.MissionState.CompleteNotPass);
                        break;
                    case Enums.CheckState.NoCheck:
                        missions = missions.Where(s => s.State == Enums.MissionState.CompleteNoCheck);
                        break;
                    default:
                        break;
                }
            }
            var orders = db.Orders.Include(s => s.User).Where(s => (missions.Select(m => m.DataID).ToList()).Contains(s.ID)).ToList();
            var pids = orders.Select(o => o.ProductID).ToList();
            var products = db.Products.Where(s => pids.Contains(s.ID)).ToList();
            var list = new List<ReceivablesList>();
            foreach (var item in missions.ToList())
            {
                var o = orders.FirstOrDefault(s => s.ID == item.DataID);
                o.Products = products.FirstOrDefault(s => s.ID == o.ProductID);
                list.Add(new ReceivablesList(item, o));
            }
            var paged = list.AsQueryable().OrderBy(s => s.CreateDateTime).ToPagedList(page);
            return View(paged);
        }

        [HttpPost]
        public ActionResult Check(int id, bool result)
        {
            var mission = db.Missions.Include(s => s.User).FirstOrDefault(s => s.ID == id);
            if (mission == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有这个审核记录"));
            }
            if (mission.State != Enums.MissionState.CompleteNoCheck)
            {
                return Json(Comm.ToMobileResult("Error", "记录已完成审核"));
            }
            var account = JsonConvert.DeserializeObject<Account>(mission.MissionDetail.First().JData);
            Receivables r = new Receivables(mission.DataID, UserID, account);
            if (result)
            {
                r.Next("通过", "");
            }
            else
            {
                r.Next("不通过", "");
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "审核完成"));
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
