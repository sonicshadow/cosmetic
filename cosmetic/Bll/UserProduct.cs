using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;

namespace Cosmetic.Bll
{
    public class UserProduct
    {
        /// <summary>
        /// 循环商品，添加用户等级对应的商品
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="rank">用户等级</param>
        public static void CreateByUser(string id, Enums.UserType rank)
        {
            using (Models.ApplicationDbContext db = new ApplicationDbContext())
            {
                var product = db.Products.ToList();
                foreach (var item in product)
                {
                    var details = item.ProductDetail.FirstOrDefault(s => s.UseType == rank);
                    var userProduct = new Models.UserProduct()
                    {
                        CreateTime = DateTime.Now,
                        Min = details.Min,
                        Price = details.Price,
                        TwiceMin=details.TwiceMin,
                        ProductID = item.ID,
                        UserID = id,
                    };
                    db.UserProducts.Add(userProduct);
                }
                db.SaveChanges();
            }            
        }

        public static void CreateByPid(int pid)
        {
            using (Models.ApplicationDbContext db=new ApplicationDbContext())
            {
                var product = db.Products.Find(pid);
                var users = db.Users.Where(s => s.Type == Enums.User.Normal).ToList();
                foreach (var item in users)
                {
                    var detail = product.ProductDetail.FirstOrDefault(s => s.UseType == item.Rank);
                    var up = new Models.UserProduct()
                    {
                        CreateTime = DateTime.Now,
                        Price = detail.Price,
                        Min = detail.Min,
                        TwiceMin=detail.TwiceMin,
                        ProductID = pid,
                        UserID = item.Id
                    };
                    db.UserProducts.Add(up);
                }
                db.SaveChanges();
            }
        }

        public void CreateByPidAndUser(string id, int pid)
        {
            using (Models.ApplicationDbContext db = new ApplicationDbContext())
            {
                var product = db.Products.Find(pid);
                var user = db.Users.Find(id);
                var detail = product.ProductDetail.FirstOrDefault(s => s.UseType == user.Rank);
                var up = new Models.UserProduct()
                {
                    CreateTime = DateTime.Now,
                    Price = detail.Price,
                    Min = detail.Min,
                    TwiceMin=detail.TwiceMin,
                    ProductID = pid,
                    UserID = id,
                };
                db.UserProducts.Add(up);
                db.SaveChanges();
            }
        }
    }
}