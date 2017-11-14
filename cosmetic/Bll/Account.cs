using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;
using System.Web.Mvc;

namespace Cosmetic.Bll
{
    public class Account
    {
        public static void Create(Models.Account account, string akname, int pay)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var akid = db.AccountKinds
                        .FirstOrDefault(s => s.Name == akname);
                if (akid == null)
                {
                    throw new Exception($"没有{akname}这个科目");
                }
                var payee = db.Payees.FirstOrDefault(s => s.ID == pay);
                if (payee == null)
                {
                    throw new Exception("请选择公司账号");
                }
                payee = payee == null ? new Payee() : payee;
                var a = new Models.Account()
                {
                    BankAccount = payee.Name,
                    BankCard = payee.BankCard,
                    BankName = payee.Bank,
                    Amount = account.Amount,
                    Remark = account.Remark,
                    UpdateDateTime = account.UpdateDateTime,
                    AccountKindID = akid.ID,
                    Fee = account.Fee,
                    Trader = account.Trader,
                    IsDelete = false,
                    AllowDelete = false,
                    TraderType = account.TraderType,
                    UserType = account.UserType
                };
                db.Accounts.Add(a);
                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        /// <summary>
        /// 会计科目添加商品收入
        /// </summary>
        /// <param name="amount">订单总金额</param>
        /// <param name="remark"></param>
        /// <param name="accountKindID"></param>
        /// <param name="parent"></param>
        public static void CreateIncome(Models.Account account, string parent, int pay)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (string.IsNullOrWhiteSpace(parent))
                {
                    try
                    {
                        Create(account, "商品类收入", pay);
                    }
                    catch (Exception)
                    {
                        throw new Exception("下单失败");
                    }
                }
            }
        }

        public static List<SelectListItem> GetPayeeInSelect()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var accounts = GetAccount(null, null, null, null, null).
                    GroupBy(s => new { s.BankCard, s.BankName })
                    .Select(s => new BankCapital()
                    {
                        BankAccount = s.Key.BankCard,
                        BankName = s.Key.BankName,
                        Total = s.Where(w=>!w.IsDelete).Sum(q => q.Total)
                    });
                var list = db.Payees;
                var selectItem = new List<SelectListItem>();
                foreach (var item in list)
                {
                    var account = accounts.FirstOrDefault(s => s.BankAccount == item.BankCard && s.BankName == item.Bank);
                    var total = account == null ? 0 : account.Total;
                    selectItem.Add(new SelectListItem()
                    {
                        Value = item.ID.ToString(),
                        Text = $"{item.Name}({item.BankCard} {item.Bank}) ￥{total}",
                    });
                }
                return selectItem;
            }
        }

        /// <summary>
        /// 获取全部结存
        /// </summary>
        /// <param name="trader">交易人</param>
        /// <param name="bankName">银行名称</param>
        /// <param name="bandCard">银行卡号</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static List<AccountViewModel> GetAccount(string trader, string bankName, string bandCard, DateTime? start, DateTime? end)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<AccountViewModel> list = new List<AccountViewModel>();
                var accounts = db.Accounts.AsQueryable();
                decimal amount = 0;
                foreach (var item in accounts.ToList())
                {
                    var total = (item.AccountKind.Type != Enums.AccountKindType.Pay ? item.Amount : -item.Amount);
                    amount = item.IsDelete ? amount : amount + total - item.Fee;
                    list.Add(new AccountViewModel()
                    {
                        Amount = amount,
                        BankCard = item.BankCard,
                        BankName = item.BankName,
                        Rank = item.UserType,
                        Time = item.UpdateDateTime,
                        AccountKind = item.AccountKind,
                        Total = total - item.Fee,
                        Trader = item.Trader,
                        TraderType = item.TraderType,
                        IsDelete = item.IsDelete,
                        Remark=item.Remark,
                    });
                }
                if (!string.IsNullOrWhiteSpace(bankName))
                {
                    list = list.Where(s => s.BankName == bankName).ToList();
                }
                if (!string.IsNullOrWhiteSpace(bandCard))
                {
                    list = list.Where(s => s.BankCard == bandCard).ToList();
                }
                if (start.HasValue)
                {
                    list = list.Where(s => s.Time >= start).ToList();
                }
                if (end.HasValue)
                {
                    list = list.Where(s => s.Time <= end).ToList();
                }
                var supplierids = list.Where(s => s.TraderType == Enums.TraderType.Supplier).Select(s => s.Trader);
                var suppliers = db.Suppliers.Where(s => supplierids.Contains(s.ID.ToString())).ToList();
                var userids = list.Where(s => s.TraderType == Enums.TraderType.User).Select(s => s.Trader);
                var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
                var payeeids = list.Where(s => s.TraderType == Enums.TraderType.Payee).Select(s => s.Trader);
                var payees = db.Payees.Where(s => payeeids.Contains(s.ID.ToString())).ToList();
                if (!string.IsNullOrWhiteSpace(trader))
                {
                    string dataid = null;
                    Enums.TraderType? type = null;
                    var user = users.FirstOrDefault(s => s.UserName == trader);
                    if (user != null)
                    {
                        dataid = user.Id;
                        type = Enums.TraderType.User;
                    }
                    var supplier = suppliers.FirstOrDefault(s => s.Name == trader);
                    if (supplier != null)
                    {
                        dataid = supplier.ID.ToString();
                        type = Enums.TraderType.Supplier;
                    }
                    var payee = payees.FirstOrDefault(s => s.Name == trader);
                    if (payee != null)
                    {
                        dataid = payee.ID.ToString();
                        type = Enums.TraderType.Payee;
                    }
                    var t = list.Where(s => s.Trader == trader);
                    if (t.Count() > 0)
                    {
                        dataid = trader;
                        type = Enums.TraderType.Other;
                    }
                    list = list.Where(s => s.Trader == dataid && s.TraderType == type).ToList();
                }
                foreach (var item in list)
                {
                    string name = item.Trader;
                    switch (item.TraderType)
                    {
                        case Enums.TraderType.Other:
                            break;
                        case Enums.TraderType.Payee:
                            {
                                name = payees.FirstOrDefault(s => s.ID.ToString() == item.Trader).Name;
                            }
                            break;
                        case Enums.TraderType.Supplier:
                            {
                                name = suppliers.FirstOrDefault(s => s.ID.ToString() == item.Trader).Name;
                            }
                            break;
                        case Enums.TraderType.User:
                            {
                                name = users.FirstOrDefault(s => s.Id == item.Trader).RealName;
                            }
                            break;
                        default:
                            break;
                    }
                    item.Trader = name;
                }
                return list;
            }
        }

        /// <summary>
        /// 获取个别账号结存
        /// </summary>
        /// <param name="bankName"></param>
        /// <param name="bandCard"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static decimal GetPayeeAccount(string bankName, string bandCard)
        {
            var list = GetAccount(null, bankName, bandCard, null,null);
            if (list.Count > 0)
            {
                return list.Last().Amount;
            }
            else
            {
                return 0;
            }
        }
    }
}