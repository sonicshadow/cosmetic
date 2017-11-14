using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class ProductDetailController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult GetByPIDAndType(int pid, Enums.UserType type)
        {
            var productDetails = db.ProductDetails.FirstOrDefault(s => s.ProductID == pid && s.UseType == type);
            if (productDetails != null)
            {
                return Json(Comm.ToMobileResult("Success", "成功", new
                {
                    Data = new ProductDetail()
                    {
                        ID = productDetails.ID,
                        Min = productDetails.Min,
                        Price=productDetails.Price,
                        ProductID=productDetails.ProductID,
                        UseType=productDetails.UseType
                    }
                }));
            }
            else
            {
                return Json(Comm.ToMobileResult("Error", "失败"));
            }
        }

        public ActionResult GetByNumberAndType(string number, Enums.UserType type)
        {
            var p = db.Products.FirstOrDefault(s => s.Number == number);
            if (p == null)
            {
                return Json(Comm.ToMobileResult("Error", "条形码有误"));
            }
            var productDetails = db.ProductDetails.FirstOrDefault(s => s.ProductID == p.ID && s.UseType == type);
            if (productDetails != null)
            {
                productDetails.Product = new Product();
                return Json(Comm.ToMobileResult("Success", "成功", new { Data = productDetails }));
            }
            else
            {
                return Json(Comm.ToMobileResult("Error", "失败"));
            }
        }

    }
}
