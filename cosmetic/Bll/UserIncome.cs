using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;
using Cosmetic.Enums;

namespace Cosmetic.Bll
{
    public class UserIncome
    {
        public static void CreateUserIncome(Models.Order order)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var orderUser = db.Users.FirstOrDefault(s => s.Id == order.UserID);
                if (!string.IsNullOrWhiteSpace(orderUser.Recommend))
                {
                    var recomend = db.Users.FirstOrDefault(s => s.Id == orderUser.Recommend);
                    //根据等级高低判断收入类型
                    //推荐人等级等于或低于订单人等级，且推荐人!=零售
                    if (recomend.Rank != UserType.Retailer && recomend.Rank.GetHashCode() >= orderUser.Rank.GetHashCode())
                    {
                        //公司给推荐人的钱
                        var userIncome = new Models.UserIncome()
                        {
                            Type = UserIncomeType.Bonus,
                            CreateDateTime = DateTime.Now,
                            IsPay = false,
                            DateID = order.ID,
                            RecommendID = order.UserID,
                            Amount= decimal.Multiply(order.Total, (decimal)0.1),
                            UserID= orderUser.Recommend,
                        };
                        db.UserIncomes.Add(userIncome);
                        db.SaveChanges();
                    }
                    //推荐人等级 = 订单人等级，且推荐人 == 零售
                    if (recomend.Rank == UserType.Retailer && recomend.Rank == orderUser.Rank)
                    {
                        //发货人给推荐人的钱
                        var userIncome = new Models.UserIncome()
                        {
                            Type = UserIncomeType.RetailBonus,
                            CreateDateTime = DateTime.Now,
                            IsPay = false,
                            DateID = order.ID,
                            RecommendID = orderUser.Recommend,
                            Amount = decimal.Multiply(order.Count, 3000),
                            UserID =orderUser.Parent,
                        };
                        db.UserIncomes.Add(userIncome);
                        db.SaveChanges();
                    }
                }
            }
        }

        //public static void CreateUserIncome(Order order)
        //{
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        var orderUser = db.Users.FirstOrDefault(s => s.Id == order.UserID);

        //        if (!string.IsNullOrWhiteSpace(orderUser.Recommend))
        //        {
        //            var recomend = db.Users.FirstOrDefault(s => s.Id == orderUser.Recommend);
        //            //根据等级高低判断收入类型
        //            var userIncomeType = UserIncomeType.Bonus;
        //            //推荐人等级>订单人等级
        //            if (recomend.Rank.GetHashCode() < orderUser.Rank.GetHashCode())
        //            {
        //                userIncomeType = UserIncomeType.Differences;
        //            }
        //            //推荐人等级=订单人等级，且推荐人!=零售
        //            if (recomend.Rank != UserType.Retailer && recomend.Rank == orderUser.Rank)
        //            {
        //                userIncomeType = UserIncomeType.Bonus;
        //            }
        //            //推荐人和订单人都是零售
        //            if (recomend.Rank == UserType.Retailer && orderUser.Rank == UserType.Retailer)
        //            {
        //                userIncomeType = UserIncomeType.RetailBonus;
        //            }
        //            //给推荐人的钱
        //            var userIncome = new Models.UserIncome()
        //            {
        //                Type = userIncomeType,
        //                CreateDateTime = DateTime.Now,
        //                IsPay = false,
        //                DateID = order.ID,
        //                RecommendID = order.UserID,
        //            };
        //            //情况2，公司打10%给推荐人
        //            if (userIncomeType == UserIncomeType.Bonus)
        //            {
        //                userIncome.Amount = decimal.Multiply(order.Total, (decimal)0.1);
        //                userIncome.UserID = orderUser.Recommend;
        //                var parentIncome = new Models.UserIncome()
        //                {
        //                    Type = UserIncomeType.Differences,
        //                    CreateDateTime = DateTime.Now,
        //                    IsPay = false,
        //                    DateID = order.ID,
        //                    RecommendID = order.UserID,
        //                    Amount = order.Total,
        //                    UserID = orderUser.Parent,
        //                };
        //                db.UserIncomes.Add(parentIncome);
        //            }
        //            //情况3，公司打3000给推荐人，剩下的打给上级发货人
        //            if (userIncomeType == UserIncomeType.RetailBonus)
        //            {
        //                userIncome.Amount = 3000 * order.Count;
        //                userIncome.UserID = orderUser.Recommend;
        //                var parentIncome = new Models.UserIncome()
        //                {
        //                    UserID = orderUser.Parent,
        //                    Type = userIncomeType,
        //                    IsPay = false,
        //                    DateID = order.ID,
        //                    RecommendID = order.UserID,
        //                    CreateDateTime = DateTime.Now,
        //                    Amount = order.Total - userIncome.Amount,
        //                };
        //                db.UserIncomes.Add(parentIncome);
        //            }
        //            db.UserIncomes.Add(userIncome);
        //            db.SaveChanges();
        //        }
        //    }
        //}
    }
}