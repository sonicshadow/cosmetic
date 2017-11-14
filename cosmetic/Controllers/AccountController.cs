using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Cosmetic.Models;
using Cosmetic.Enums;
using System.Data.Entity;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext db = new ApplicationDbContext();
        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = db.Users.FirstOrDefault(s => s.UserName == model.UserName || s.PhoneNumber == model.UserName);
            if (user.Type== Enums.User.Normal && user.State != UserState.Actived)
            {
                ModelState.AddModelError("", "账号未激活");
                return View(model);
            }
            //SignInManager.SignIn()
            var result = await SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if ((string.IsNullOrWhiteSpace(user.Bank)
                        || string.IsNullOrWhiteSpace(user.BankCode)
                        || string.IsNullOrWhiteSpace(user.Address)
                        || string.IsNullOrWhiteSpace(user.BankCard))
                        && user.Type == Enums.User.Normal)
                    {
                        returnUrl = Url.Action("ResetPassword", "Account");
                    }
                    if (string.IsNullOrWhiteSpace(returnUrl))
                    {
                        switch (user.Type)
                        {
                            case Enums.User.Normal:
                                returnUrl = Url.Action("Index", "Order");
                                break;
                            case Enums.User.Admin:
                            default:
                                returnUrl = Url.Action("Index", "Product");
                                break;
                        }
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "账号或密码错误");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // 要求用户已通过使用用户名/密码或外部登录名登录
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 以下代码可以防范双重身份验证代码遭到暴力破解攻击。
            // 如果用户输入错误代码的次数达到指定的次数，则会将
            // 该用户帐户锁定指定的时间。
            // 可以在 IdentityConfig 中配置帐户锁定设置
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "代码无效。");
                    return View(model);
            }
        }

        /// <summary>
        /// 管理员的注册会员
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = SysRole.UserManageCreate)]
        public ActionResult Register()
        {
            ViewBag.Sidebar = "用户管理";
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = SysRole.UserManageCreate)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Sidebar = "用户管理";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Phone,
                    Address = model.Address,
                    Bank = model.Bank,
                    BankCard = model.BankCard,
                    PhoneNumber = model.Phone,
                    Rank = model.Type,
                    BankCode = model.BankCode,
                    RegisterDateTime = DateTime.Now,
                    LastLoginDateTime = DateTime.Now,
                    RealName = model.Name,
                    Type = Enums.User.Normal,
                    Recommend = model.Recommend,
                    State = UserState.Inactive,
                };
                var password = Comm.Random.Next(100000, 999999).ToString();
                try
                {
                    user = Bll.User.SetParentByRecommend(user);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("Recommend", ex.Message);
                    return View(model);
                }
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    Bll.UserProduct.CreateByUser(user.Id, user.Rank);
                    return RedirectToAction("UserSetting", "Account", new { id = user.Id });
                }
                ModelState.AddModelError("", result.Errors.First());
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }


        public List<RegisterViewModel> DaoRu()
        {
            var user = XDocument.Load("D:/Project/cosmetic/users.xml").Root.Elements("user").ToList();
            var list = new List<RegisterViewModel>();

            foreach (var e in user)
            {
                var sss = (string)e.Element("Bank");
                UserType type = UserType.One;
                switch ((string)e.Element("Rank"))
                {
                    case "特级":
                        type = UserType.Premium;
                        break;
                    case "一级":
                        type = UserType.One;
                        break;
                    case "二级":
                        type = UserType.Two;
                        break;
                    case "三级":
                        type = UserType.Three;
                        break;
                    case "零售":
                        type = UserType.Retailer;
                        break;
                    default:
                        break;
                }
                list.Add(new RegisterViewModel()
                {
                    Phone = (string)e.Element("PhoneNumber"),
                    Bank = (string)e.Element("Bank"),
                    BankCard = (string)e.Element("BankCard"),
                    Name = (string)e.Element("RealName"),
                    Recommend = (string)e.Element("Recommend"),
                    Type = type,
                    IDCard = (string)e.Element("IDCard"),
                    Time = (DateTime)e.Element("RegisterDateTime"),
                });

            }
            return list;
        }

        [HttpPost]
        public ActionResult dapcu()
        {
            var daoRu = DaoRu();
            var users = db.Users.Where(s => s.Type == Enums.User.Normal);
            foreach (var item in users)
            {
                var user = daoRu.FirstOrDefault(s => s.Name == item.RealName);
                item.IDCard = user.IDCard;
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "cg", new { Data = users.Count() }));
        }

        public void addUser(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Phone,
                Address = model.Address,
                Bank = model.Bank,
                BankCard = model.BankCard,
                IDCard = model.IDCard,
                PhoneNumber = model.Phone,
                Rank = model.Type,
                BankCode = model.BankCode,
                RegisterDateTime = model.Time,
                LastLoginDateTime = DateTime.Now,
                RealName = model.Name,
                Type = Enums.User.Normal,
                Recommend = model.Recommend,
                State = UserState.Inactive,
            };
            try
            {
                user = Bll.User.SetParentByRecommend(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            var result = UserManager.CreateAsync(user, "12345678");
            if (result.Result.Succeeded)
            {
                Bll.UserProduct.CreateByUser(user.Id, user.Rank);
            }
            else
            {
                throw new Exception(result.Result.Errors.First());
            }
        }

        public ActionResult UserSetting(string id)
        {
            ViewBag.Sidebar = "用户管理";
            var products = db.Products.Select(s => new SelectListItem()
            {
                Value = s.ID.ToString(),
                Text = s.Name,
            });
            ViewBag.ProductList = products.ToList();
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
            var model = new UserSettingModel()
            {
                Order = new Order()
                {
                    UserID = id,
                },
                UserProduct = db.UserProducts.Include(s => s.Product).Include(s => s.User).Where(s => s.UserID == id).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult UserSetting(UserSettingModel model, bool saleOrder, decimal salePrice,
            decimal saleFee = 0, int salePayeeID = 0, decimal fee = 0, int PayeeID = 0)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == model.Order.UserID);
            //修改商品的最小数量
            var ups = db.UserProducts
                .Where(s => s.UserID == model.Order.UserID)
                .ToList();
            foreach (var item in ups)
            {
                var up = model.UserProduct.FirstOrDefault(s => s.ID == item.ID);
                item.Min = up.Min;
                item.TwiceMin = up.TwiceMin;
                item.Price = up.Price;
            }
            //添加订单
            //下单给推荐人
            if (saleOrder)
            {
                var rOrder = new Order()
                {
                    Code = $"{DateTime.Now.ToString("yyyyMMdd")}{Comm.Random.Next(10000, 99999)}",
                    Count = 1,
                    CreateDateTime = DateTime.Now,
                    ReceiptDateTime = DateTime.Now,
                    Send = 0,
                    ProductID = model.Order.ProductID,
                    Price = salePrice,
                    Total = salePrice,
                    UserID = user.Id,
                    IsPay = string.IsNullOrWhiteSpace(user.Recommend) ? true : false,
                    State = OrderState.Pay,
                    ParentUser = user.Recommend,
                };
                if (salePayeeID != 0)
                {
                    var rPay = db.Payees.FirstOrDefault(s => s.ID == salePayeeID);
                    rOrder.BankAccount = rPay.Name;
                    rOrder.BankCard = rPay.BankCard;
                    rOrder.BankName = rPay.Bank;
                    try
                    {
                        var account = new Account()
                        {
                            Remark = $"{user.RealName}向公司进货",
                            Fee = saleFee,
                            UserType = user.Rank,
                            Trader = user.Id,
                            TraderType = TraderType.User,
                            Amount = rOrder.Total,
                            UpdateDateTime = DateTime.Now,
                        };
                        Bll.Account.CreateIncome(account, rOrder.ParentUser, salePayeeID);
                    }
                    catch (Exception ex)
                    {
                        return Json(Comm.ToMobileResult("Error", ex.Message));
                    }
                }
                db.Orders.Add(rOrder);
            }
            //下单给发货人
            if (model.Order.Count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "下单数量不能小于0"));
            }
            var order = new Order()
            {
                Code = $"{DateTime.Now.ToString("yyyyMMdd")}{Comm.Random.Next(1000, 9999)}",
                Count = model.Order.Count,
                CreateDateTime = DateTime.Now,
                ReceiptDateTime = DateTime.Now,
                Send = 0,
                ProductID = model.Order.ProductID,
                Price = model.Order.Price,
                Total = model.Order.Total,
                UserID = user.Id,
                IsPay = string.IsNullOrWhiteSpace(user.Recommend) ? true : false,
                State = OrderState.Pay,
                ParentUser = user.Parent,
            };
            if (PayeeID != 0)
            {
                var pay = db.Payees.FirstOrDefault(s => s.ID == PayeeID);
                order.BankAccount = pay.Name;
                order.BankCard = pay.BankCard;
                order.BankName = pay.Bank;
            }
            db.Orders.Add(order);
            //会计科目添加
            try
            {
                var account = new Account()
                {
                    Remark = $"{user.RealName}向公司进货",
                    Fee = fee,
                    UserType = user.Rank,
                    Trader = user.Id,
                    TraderType = TraderType.User,
                    Amount = order.Total,
                    UpdateDateTime = DateTime.Now,
                };
                Bll.Account.CreateIncome(account, order.ParentUser, PayeeID);
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Error", ex.Message));
            }
            //修改订单库存
            var stock = ups.FirstOrDefault(s => s.UserID == user.Id && s.ProductID == order.ProductID);
            stock.Sum = stock.Sum + order.Count;
            //激活用户
            user.State = UserState.Actived;
            //修改密码
            var password = Comm.Random.Next(100000, 999999).ToString();
            UserManager.RemovePassword(user.Id);
            UserManager.AddPassword(user.Id, password);
            //发信息
            if (user.Rank != UserType.Retailer)
            {
                var isms = new CnSms();
                var ismsResult = isms.Send(user.PhoneNumber, $"恭喜你成功注册成为汉焱祖方的会员，会员号：{user.UserName}，密码：{password}，请登录网址:http://hanyanzufang.com/Account/Login ,修改登录密码注册资料.本公司任何时候不需要用户银行密码慎防受骗。");
                if (ismsResult.IsSuccess)
                {
                    db.SaveChanges();
                    //创建推荐人和上级发货人的收入
                    Bll.UserIncome.CreateUserIncome(order);
                    return Json(Comm.ToMobileResult("Success", "成功"));
                }
                else
                {
                    return Json(Comm.ToMobileResult("Error", ismsResult.Message));
                }
            }
            else
            {
                db.SaveChanges();
                //创建推荐人和上级发货人的收入
                if (string.IsNullOrWhiteSpace(order.ParentUser))
                {
                    Bll.UserIncome.CreateUserIncome(order);
                }
                return Json(Comm.ToMobileResult("Success", "成功"));
            }
        }

        [Authorize(Roles = SysRole.AdminManageCreate)]
        public ActionResult RegisterAdmin()
        {
            ViewBag.Sidebar = "管理员管理";
            ViewBag.roleGroup = db.RoleGroups.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.ID.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = SysRole.AdminManageCreate)]
        public async Task<ActionResult> RegisterAdmin(RegisterAdmin model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,

                    RegisterDateTime = DateTime.Now,
                    LastLoginDateTime = DateTime.Now,
                    Type = Enums.User.Admin,
                    Address = model.Address,
                    Bank = model.Bank,
                    BankCard = model.BankCard,
                    RealName = model.RealName,
                    PhoneNumber = model.Phone,
                    IDCard = model.IDCard,
                    RoleGroupID = model.RoleGroupID,
                };
                var password = model.IDCard.Remove(0, model.IDCard.Length - 6);
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var roleGroup = db.RoleGroups.FirstOrDefault(s => s.ID == model.RoleGroupID);
                    var roles = roleGroup.Roles.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
                    UserManager.AddToRoles(user.Id, roles);
                    return RedirectToAction("Index", "InsideUsers");
                }
                AddErrors(result);
            }
            ViewBag.roleGroup = db.RoleGroups.Select(s => new SelectListItem
            {
                Text = s.Name,
                Value = s.ID.ToString()
            }).ToList();
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        /// <summary>
        /// 代理商的注册会员
        /// </summary>
        /// <returns></returns>
        public ActionResult RegisterByUser()
        {
            try
            {
                InitRegisterByUser();
            }
            catch (Exception ex)
            {
                this.ToError("权限不足", ex.Message);
            }
            return View();
        }

        private void InitRegisterByUser()
        {
            ViewBag.Sidebar = "我的下级";
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            var temp = Enum.GetValues(typeof(Enums.UserType));
            Dictionary<string, Enums.UserType> selRankSouce = new Dictionary<string, UserType>();
            //获取该代理商可以选择的用户级别
            foreach (Enums.UserType item in temp)
            {
                if (user.Rank.GetHashCode() < item.GetHashCode())
                {
                    selRankSouce.Add(item.GetDisplayName(), item);
                }
            }
            ViewBag.Rank = new SelectList(selRankSouce, "Value", "Key");
            //获取该代理商可以选择的团队人员
            var team = db.Users
                .Where(s => s.AllParent.Contains(UserID) || s.Recommend == UserID)
                .Select(s => new { s.Id, s.RealName, s.PhoneNumber, s.Rank }).ToList()
                .Select(s => new { Key = $"{s.RealName}[{s.PhoneNumber}] {s.Rank.GetDisplayName()}", Value = s.Id }).ToList();
            team.Insert(0, new { Key = $"{user.RealName}[{user.PhoneNumber}] {user.Rank.GetDisplayName()}", Value = user.Id });
            ViewBag.Team = new SelectList(team, "Value", "Key");
            switch (user.Rank)
            {
                case UserType.Retailer:
                    {
                        throw new Exception($"{user.Rank.GetDisplayName()}没有权限建立下级");
                    }
                    break;
                default:
                    break;
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterByUser(RegisterByUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = db.Users.Where(s => s.Id == UserID || s.Id == model.Recommend);
                var my = users.FirstOrDefault(s => s.Id == UserID);
                var recommend = users.FirstOrDefault(s => s.Id == model.Recommend);
                if (recommend.Rank == UserType.Retailer && model.Rank != UserType.Retailer)
                {
                    ModelState.AddModelError("Rank", "零售商只能推荐零售商");
                }
                else
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Phone,
                        PhoneNumber = model.Phone,
                        Rank = model.Rank,
                        RegisterDateTime = DateTime.Now,
                        LastLoginDateTime = DateTime.Now,
                        RealName = model.Name,
                        Type = Enums.User.Normal,
                        Recommend = recommend.UserName,
                        State = UserState.Inactive,
                        Parent = recommend.Id,
                        AllParent = $"{my.AllParent}[{UserID}]"
                    };
                    var password = Comm.Random.Next(100000, 999999).ToString();
                    try
                    {
                        user = Bll.User.SetParentByRecommend(user);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Recommend", ex.Message);
                        InitRegisterByUser();
                        return View(model);
                    }
                    var result = await UserManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        Bll.UserProduct.CreateByUser(user.Id, user.Rank);
                        return RedirectToAction("Team", "UserProduct");
                    }
                    AddErrors(result);
                }
            }
            InitRegisterByUser();
            return View(model);
        }


        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Phone);
                if (user == null)
                {
                    ModelState.AddModelError("Phone", "用户不存在");
                    return View(model);
                }
                var password = Comm.Random.Next(100000, 999999).ToString();
                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, password);
                ISms sms = new CnSms();
                var smsResult = sms.Send(user.PhoneNumber, $"尊敬的汉焱祖方用户，您已经成功重置默认密码，密码：{password}");
                if (smsResult.IsSuccess)
                {
                    return RedirectToAction("Login", "Account");
                }
                ModelState.AddModelError("", smsResult.Message);
            }
            // 如果我们进行到这一步时某个地方出错，则重新显示表单
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        public ActionResult ResetPassword()
        {
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            var layout = false;
            if ((string.IsNullOrWhiteSpace(user.Bank)
                       || string.IsNullOrWhiteSpace(user.BankCode)
                       || string.IsNullOrWhiteSpace(user.Address)
                       || string.IsNullOrWhiteSpace(user.BankCard))
                       && user.Type == Enums.User.Normal)
            {
                layout = true;
            }
            ViewBag.Layout = layout;
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            var layout = false;
            if ((string.IsNullOrWhiteSpace(user.Bank)
                       || string.IsNullOrWhiteSpace(user.BankCode)
                       || string.IsNullOrWhiteSpace(user.Address)
                       || string.IsNullOrWhiteSpace(user.BankCard))
                       && user.Type == Enums.User.Normal)
            {
                layout = true;
            }
            ViewBag.Layout = layout;
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isValid = new Regex("^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,16}$").IsMatch(model.NewPassword);
            if (!isValid)
            {
                ModelState.AddModelError("NewPassword", "密码必须是数字加字母");
                return View(model);
            }
            var result = UserManager.ChangePassword(UserID, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("Details", "User");
            }
            ModelState.AddModelError("OldPassword", result.Errors.First());
            return View(model);
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 请求重定向到外部登录提供程序
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // 生成令牌并发送该令牌
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 如果用户已具有登录名，则使用此外部登录提供程序将该用户登录
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // 如果用户没有帐户，则提示该用户创建帐户
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region 帮助程序
        // 用于在添加外部登录名时提供 XSRF 保护
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}