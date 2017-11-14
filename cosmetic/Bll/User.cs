using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;
using System.Data.Entity;

namespace Cosmetic.Bll
{
    public class User
    {

        public static TeamModel GetUserTeam(string name)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = new ApplicationUser();
                var users = db.Users.Where(s => s.Type == Enums.User.Normal).ToList();
                if (!string.IsNullOrWhiteSpace(name) && name != "公司")
                {
                    user = users.FirstOrDefault(s => s.UserName == name);
                }
                else
                {
                    user.Id = null;
                }
                var team = new TeamModel() { User = user };
                Action<TeamModel> action = null;
                action = u =>
                {
                    var next = users.Where(s => s.Parent == u.User.Id).ToList();
                    if (next.Count > 0)
                    {
                        var child = next.Select(s => new TeamModel()
                        {
                            User = s,
                            Child = new List<TeamModel>()
                        }).ToList();
                        u.Child = child;
                        foreach (var items in child)
                        {
                            action(items);
                        }
                    }
                };
                action(team);
                return team;
            }
        }



        public static List<ApplicationUser> GetDirectPush(string name)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var list = new List<ApplicationUser>();
                if (string.IsNullOrWhiteSpace(name) || name == "公司")
                {
                    list = db.Users.Where(s => s.Recommend == null && s.Type == Enums.User.Normal).ToList();
                }
                else
                {
                    var user = db.Users.FirstOrDefault(s => s.UserName == name);
                    list = db.Users.Where(s => s.Recommend == user.Id && s.Type == Enums.User.Normal).ToList();
                }
                return list.OrderBy(s => s.RegisterDateTime).ToList();
            }
        }

        public static ApplicationUser SetParentByRecommend(ApplicationUser user)
        {
            user.Recommend = user.Recommend.Trim();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (!string.IsNullOrWhiteSpace(user.Recommend) && user.Recommend != "公司")
                {
                    var recommend = db.Users.FirstOrDefault(s => s.UserName == user.Recommend);
                    if (recommend == null)
                    {
                        throw new Exception("找不到推荐人");
                    }
                    //要判断用户是否已经冻结了
                    if (user.State == Enums.UserState.Frozen)
                    {
                        throw new Exception("推荐人已被冻结");
                    }
                    user.Recommend = recommend.Id;   //推荐人设置
                    //上级发货人设置
                    if (user.Rank == Enums.UserType.Premium)
                    {
                        user.Parent = null;
                        user.AllParent = null;
                    }
                    else
                    {
                        if (user.Rank == recommend.Rank)//同级
                        {
                            user.Parent = recommend.Parent;
                            user.AllParent = recommend.AllParent;
                        }
                        if (user.Rank.GetHashCode() > recommend.Rank.GetHashCode())//下级
                        {
                            user.Parent = recommend.Id;
                            user.AllParent = $"{recommend.AllParent}[{user.Parent}]";
                        }
                        if (user.Rank.GetHashCode() < recommend.Rank.GetHashCode())//上级
                        {
                            Action<string> action = null;
                            action = parent =>
                            {
                                if (!string.IsNullOrWhiteSpace(parent))
                                {
                                    var parentUser = db.Users.FirstOrDefault(s => s.Id == parent);
                                    if (parentUser.Rank == user.Rank)
                                    {
                                        user.Parent = parentUser.Parent;
                                        user.AllParent = recommend.AllParent;
                                    }
                                    if (user.Rank.GetHashCode() > parentUser.Rank.GetHashCode())
                                    {
                                        user.Parent = parentUser.Id;
                                        user.AllParent = $"{recommend.AllParent}[{user.Parent}]";
                                    }
                                    if (user.Rank.GetHashCode() < parentUser.Rank.GetHashCode())
                                    {
                                        action(parentUser.Parent);
                                    }
                                };
                            };
                            action(recommend.Parent);
                        }
                    }
                }
                else
                {
                    user.Recommend = null;
                    user.Parent = null;
                    user.AllParent = null;
                }
                return user;
            }
        }
    }
}