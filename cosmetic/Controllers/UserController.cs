using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using PagedList;
using Microsoft.AspNet.Identity.Owin;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        private void Sidebar()
        {

            ViewBag.Sidebar = "用户管理";
        }

        // GET: User
        [Authorize(Roles = SysRole.UserManageRead)]
        public ActionResult Index(string username, string parentname, Enums.UserType? rank, Enums.UserState? state, int page = 1)
        {
            Sidebar();
            ViewBag.InactiveCount = db.Users.Where(s => s.Type == Enums.User.Normal)
                .Count(s => s.State == Enums.UserState.Inactive);
            var users = GetUser(username, parentname, state, rank);
            var paged = users.AsQueryable()
                .OrderBy(s => s.RegisterDateTime)
                .ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Count = users.Count;
            }
            return View(paged);
        }
        public ActionResult IndexExport(string username, string parentname, Enums.UserState? state, Enums.UserType? rank)
        {
            this.AddExcelExportHead($"所有用户{DateTime.Now:yyyyMMdd}");
            return View(GetUser(username, parentname, state, rank).OrderBy(s => s.RegisterDateTime));
        }

        public List<UserList> GetUser(string username, string parentname, Enums.UserState? state, Enums.UserType? rank)
        {
            var users = db.Users.Where(s => s.Type == Enums.User.Normal);
            if (!string.IsNullOrWhiteSpace(parentname))
            {
                if (parentname == "公司")
                {
                    users = users.Where(s => s.Parent == null);
                }
                else
                {
                    var par = users.FirstOrDefault(s => s.UserName == parentname);
                    par = par == null ? new ApplicationUser() : par;
                    users = users.Where(s => s.Parent == par.Id);
                }
            }
            var parentsids = users.Select(s => s.Parent).Distinct();
            var parents = users.Where(s => parentsids.Contains(s.Id)).ToList();
            var recommendids = users.Select(s => s.Recommend).Distinct();
            var recommends = users.Where(s => recommendids.Contains(s.Id)).ToList();
            if (!string.IsNullOrWhiteSpace(username))
            {
                users = users.Where(s => s.UserName == username);
            }
            if (state.HasValue)
            {
                users = users.Where(s => s.State == state);
            }
            if (rank.HasValue)
            {
                users = users.Where(s => s.Rank == rank.Value);
            }
            var list = new List<UserList>();
            foreach (var item in users)
            {
                var parent = parents.FirstOrDefault(s => s.Id == item.Parent);
                var recommend = recommends.FirstOrDefault(s => s.Id == item.Recommend);
                list.Add(new UserList()
                {
                    Address = item.Address,
                    Bank = item.Bank,
                    BankCard = item.BankCard,
                    ID = item.Id,
                    RegisterDateTime = item.RegisterDateTime,
                    IDCard = item.IDCard,
                    Rank = item.Rank,
                    RealName = item.RealName,
                    UserName = item.UserName,
                    UserState = item.State,
                    Parent = parent == null ? "公司" : parent.RealName,
                    Recommend = recommend == null ? "公司" : recommend.RealName,
                });
            }
            return list;
        }

        // GET: User/Details/5
        public ActionResult Details()
        {
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            var layout = false;
            if ((string.IsNullOrWhiteSpace(user.Bank)
                       || string.IsNullOrWhiteSpace(user.BankCode)
                       || string.IsNullOrWhiteSpace(user.Address)
                       || string.IsNullOrWhiteSpace(user.IDCard)
                       || string.IsNullOrWhiteSpace(user.BankCard))
                       && user.Type == Enums.User.Normal)
            {
                layout = true;
            }
            ViewBag.Layout = layout;
            var model = new UserViewModel()
            {
                ID = user.Id,
                Type = user.Type,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Bank = user.Bank,
                BankCard = user.BankCard,
                IDCard = user.IDCard,
                RealName = user.RealName,
                BankCode = user.BankCode,
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Details(UserViewModel model)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == model.ID);
            var resule = true;
            if (string.IsNullOrWhiteSpace(model.Phone))
            {
                resule = false;
                ModelState.AddModelError("Phone", "电话 字段是必需的。");
            }
            if (string.IsNullOrWhiteSpace(model.Bank))
            {
                resule = false;
                ModelState.AddModelError("Bank", "银行名称 字段是必需的。");
            }
            if (string.IsNullOrWhiteSpace(model.BankCard))
            {
                resule = false;
                ModelState.AddModelError("BankCard", "银行卡号 字段是必需的。");
            }
            if (string.IsNullOrWhiteSpace(model.BankCode))
            {
                resule = false;
                ModelState.AddModelError("BankCode", "网点联行号 字段是必需的。");
            }
            if (string.IsNullOrWhiteSpace(model.Address))
            {
                resule = false;
                ModelState.AddModelError("Address", "地址 字段是必需的。");
            }
            if (string.IsNullOrWhiteSpace(model.IDCard))
            {
                resule = false;
                ModelState.AddModelError("IDCard", "身份证号码 字段是必需的。");
            }
            if (!resule)
            {
                var layout = false;
                if ((string.IsNullOrWhiteSpace(user.Bank)
                           || string.IsNullOrWhiteSpace(user.BankCode)
                           || string.IsNullOrWhiteSpace(user.Address)
                            || string.IsNullOrWhiteSpace(user.IDCard)
                           || string.IsNullOrWhiteSpace(user.BankCard))
                           && user.Type == Enums.User.Normal)
                {
                    layout = true;
                }
                ViewBag.Layout = layout;
                return View(model);
            }
            ModifyUser(user, model);
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.Bank = model.Bank;
            user.IDCard = model.IDCard;
            user.BankCard = model.BankCard;
            user.BankCode = model.BankCode;
            db.SaveChanges();
            return RedirectToAction("Index", "Order");
        }

        // GET: User/Edit/5
        [Authorize(Roles = SysRole.UserManageEdit)]
        public ActionResult Edit(string id, bool msg = false)
        {
            Sidebar();
            var user = db.Users.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var users = db.Users.Where(s => s.Id == user.Recommend || s.Id == user.Parent);
            var parent = users.FirstOrDefault(s => s.Id == user.Parent);
            user.Parent = parent == null ? "公司" : parent.UserName;
            var recommend = users.FirstOrDefault(s => s.Id == user.Recommend);
            user.Recommend = recommend == null ? "公司" : recommend.UserName;
            var userProducts = db.UserProducts.Include(s => s.Product).Where(s => s.UserID == id).ToList();
            var model = new UserViewModel()
            {
                ID = user.Id,
                Phone = user.PhoneNumber,
                Address = user.Address,
                Bank = user.Bank,
                Parent = user.Parent,
                Rank = user.Rank,
                Recommend = user.Recommend,
                BankCard = user.BankCard,
                IDCard = user.IDCard,
                RealName = user.RealName,
                UserProduct = userProducts,
                BankCode = user.BankCode,
            };
            if (msg)
            {
                ViewBag.Message = "修改成功";
            }
            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserViewModel model)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == model.ID);
            if (model.Phone != user.UserName)
            {
                var name = db.Users.Where(s => s.UserName == model.Phone);
                if (name.Count() > 0)
                {
                    Sidebar();
                    model.UserProduct = db.UserProducts.Include(s => s.Product).Where(s => s.UserID == user.Id).ToList();
                    ModelState.AddModelError("Phone", "已有相同号码的用户，请填写其它号码");
                    return View(model);
                }
            }
            var rank = user.Rank;
            ModifyUser(user, model);
            user.PhoneNumber = model.Phone;
            user.Address = model.Address;
            user.Bank = model.Bank;
            user.BankCard = model.BankCard;
            user.BankCode = model.BankCode;
            user.UserName = model.Phone;
            if (user.Rank != model.Rank)
            {
                var ups = db.UserProducts.Include(s => s.Product).Where(s => s.UserID == user.Id);
                var pids = ups.Select(s => s.ProductID).ToList();
                var details = db.ProductDetails.Where(s => pids.Contains(s.ProductID)).ToList();
                foreach (var item in ups)
                {
                    var de = details.FirstOrDefault(s => s.UseType == model.Rank);
                    item.Min = de.Min;
                    item.TwiceMin = de.TwiceMin;
                    item.Price = de.Price;
                }
            }
            user.IDCard = model.IDCard;
            user.Rank = model.Rank;
            user.RealName = model.RealName;
            db.SaveChanges();
            user.Recommend = model.Recommend;
            try
            {
                user = Bll.User.SetParentByRecommend(user);
            }
            catch (Exception ex)
            {
                Sidebar();
                model.UserProduct = db.UserProducts.Include(s => s.Product).Where(s => s.UserID == user.Id).ToList();
                ModelState.AddModelError("Recommend", ex.Message);
                return View(model);
            }
            if (rank == Enums.UserType.Retailer && user.Rank != Enums.UserType.Retailer)
            {
                var password = Comm.Random.Next(100000, 999999).ToString();
                //UserManager.RemovePassword(user.Id);
                //UserManager.AddPassword(user.Id, password);
                //发信息
                var isms = new CnSms();
                var ismsResult = isms.Send(user.PhoneNumber, $"恭喜你成功注册成为汉焱祖方的会员，会员号：{user.UserName}，密码：{password}，请登录网址:http://hanyanzufang.com/Account/Login ,修改登录密码注册资料.本公司任何时候不需要用户银行密码慎防受骗。");
                if (ismsResult.IsSuccess)
                {
                    db.SaveChanges();
                }
                else
                {
                    ModelState.AddModelError("", ismsResult.Message);
                    return View(model);
                }
            }
            else
            {
                db.SaveChanges();
            }
            return RedirectToAction("Edit", new { id = model.ID, msg = true });
        }

        // GET: User/Delete/5
        public ActionResult Delete(string id)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == id);
            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("InsideIndex");
        }


        public ActionResult LockoutEnabled(string id, string username)
        {
            var users = db.Users.Where(s => s.Id == id || s.RealName == username || s.UserName == username);
            var newuser = users.FirstOrDefault(s => s.RealName == username || s.UserName == username);
            if (newuser == null && username != "公司")
            {
                return Json(Comm.ToMobileResult("Error", "找不到替代的用户"));
            }
            var newid = username == "公司" ? null : newuser.Id;
            var olduser = users.FirstOrDefault(s => s.Id == id);
            olduser.State = Enums.UserState.Frozen;
            olduser.LockoutEndDateUtc = DateTime.Now.AddYears(10);
            var changeUsers = db.Users.Where(s => s.Parent == olduser.Id || s.Recommend == olduser.Id || s.AllParent.Contains(olduser.Id)).ToList();
            foreach (var item in changeUsers)
            {
                if (item.Parent == olduser.Id)
                {
                    item.Parent = newid;
                }
                if (item.Recommend == olduser.Id)
                {
                    item.Recommend = newid;
                }
                if (item.AllParent.Contains(olduser.Id))
                {
                    if (newuser == null)
                    {
                        item.AllParent = null;
                    }
                    else
                    {
                        item.AllParent = $"{newuser.AllParent}[{newid}]";
                    }
                }
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }

        public ActionResult LockinAbled(string id)
        {
            var users = db.Users.FirstOrDefault(s => s.Id == id);
            users.State = Enums.UserState.Actived;
            users.LockoutEndDateUtc = DateTime.Now.AddYears(-10);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "修改成功"));
        }

        public ActionResult GetTeam(string name, bool? type, int page = 1)
        {
            Sidebar();
            var user = new List<ApplicationUser>();
            if (type.HasValue)
            {
                if (type.Value)
                {
                    user = Bll.User.GetDirectPush(name);
                }
                else
                {
                    var team = Bll.User.GetUserTeam(name);
                    Action<TeamModel> action = null;
                    action = parent =>
                    {
                        if (parent.Child.Count > 0)
                        {
                            user.AddRange(parent.Child.Select(s => s.User));
                            foreach (var item in parent.Child)
                            {
                                action(item);
                            }
                        }
                    };
                    action(team);
                }
            }
            var paged = user.AsQueryable().ToPagedList(page);
            if (paged.IsLastPage)
            {
                ViewBag.Count = user.Count();
            }
            return View(paged);

        }

        [HttpPost]
        public ActionResult GetByUserName(string username)
        {
            var user = db.Users.Where(s => (s.UserName.Contains(username) || s.RealName.Contains(username)) &&
                s.Type == Enums.User.Normal && s.State == Enums.UserState.Actived).ToList();
            var data = user.Select(s => new { s.Id, s.RealName, s.UserName });
            return Json(Comm.ToMobileResult("Success", "成功", new { Data = data }));
        }

        public ActionResult Modify(string name, int page = 1)
        {
            Sidebar();
            var modifys = db.Modifys.Include(s => s.User)
                .Where(s => s.ModifyType != Enums.ModifyType.Account);
            if (!string.IsNullOrWhiteSpace(name))
            {
                modifys = modifys.Where(s => s.User.UserName == name);
            }
            var paged = modifys.OrderByDescending(s => s.Time).ToPagedList(page);
            return View(paged);
        }

        public void CreateModify(string userid, string olddata, string newdata, Enums.ModifyType type)
        {
            if (!string.IsNullOrEmpty(olddata))
            {
                if (newdata != olddata)
                {
                    var modify = new Modify()
                    {
                        UserID = userid,
                        ModifyType = type,
                        NewData = newdata,
                        OldData = olddata,
                    };
                    Bll.Modify.Create(modify);
                }
            }
        }

        public void ModifyUser(ApplicationUser olduser, UserViewModel newuser)
        {
            CreateModify(olduser.Id, olduser.IDCard, newuser.IDCard, Enums.ModifyType.IDCard);
            //CreateModify(olduser.Id, olduser.Parent, newuser.Parent, Enums.ModifyType.Parent);
            CreateModify(olduser.Id, olduser.PhoneNumber, newuser.Phone, Enums.ModifyType.PhoneNumber);
            CreateModify(olduser.Id, olduser.Address, newuser.Address, Enums.ModifyType.Address);
            CreateModify(olduser.Id, olduser.Bank, newuser.Bank, Enums.ModifyType.Bank);
            CreateModify(olduser.Id, olduser.BankCard, newuser.BankCard, Enums.ModifyType.BankCard);
            CreateModify(olduser.Id, olduser.BankCode, newuser.BankCode, Enums.ModifyType.BankCode);
            CreateModify(olduser.Id, olduser.Rank.GetDisplayName(), newuser.Rank.GetDisplayName(), Enums.ModifyType.Rank);
            CreateModify(olduser.Id, olduser.RealName, newuser.RealName, Enums.ModifyType.RealName);
            //CreateModify(olduser.Id, olduser.Recommend, newuser.Recommend, Enums.ModifyType.Recommend);
        }
    }
}
