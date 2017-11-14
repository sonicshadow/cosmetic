using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Cosmetic.Models;
namespace Cosmetic.Controllers
{
    public class TestController : Controller
    {

        public ActionResult Index()
        {
            //发出通知
            ISms sms = new CnSms();
            var result = sms.Send("15999726944", $"【汉焱祖方】恭喜你成功注册成为汉焱祖方的会员，会员号：13553854087，密码：身份证后6位，请登录网址:http://www.hanyanzufang.com/account/login,修改登录密码注册资料.本公司任何时候不需要用户银行密码慎防受骗");
            if (result.IsSuccess)
            {
                return Json(Comm.ToMobileResult("Success", "发货成功"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(Comm.ToMobileResult("Error", result.Message), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoginTemp(string name)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(s => s.UserName == name);
                var SignInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
                SignInManager.SignIn(user, false, true);
            }

            return RedirectToAction("Index", "User");
        }


       
    }
}