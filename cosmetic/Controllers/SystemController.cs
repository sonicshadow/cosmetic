using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Microsoft.AspNet.Identity;

namespace Cosmetic.Controllers
{
    public class SystemController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public string UserID
        {
            get { return User.Identity.GetUserId(); }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Notify()
        {
            var userCount = db.Users.Where(s => s.State == Enums.UserState.Inactive).Count();
            var user = db.Users.FirstOrDefault(s => s.Id == UserID);
            int orderCount = 0;
            switch (user.Type)
            {
                case Enums.User.Normal:
                    orderCount = db.Orders
                        .Where(s => s.ParentUser == UserID
                        && (s.State == Enums.OrderState.Pay 
                            || s.State == Enums.OrderState.UnPay))
                        .Count();
                    break;
                case Enums.User.Admin:
                default:
                    orderCount = db.Orders
                      .Where(s => s.ParentUser == null
                      && s.State == Enums.OrderState.Pay)
                      .Count();
                    break;
            }
            return Json(new
            {
                UserCount = userCount,//待激活用户数
                OrderCount = orderCount//待发货用户数
            }, JsonRequestBehavior.AllowGet);
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