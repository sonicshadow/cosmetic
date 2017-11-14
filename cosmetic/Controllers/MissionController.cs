using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Cosmetic.Enums;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class MissionController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }
        private void Sidebar()
        {
            ViewBag.Sidebar = "任务管理";
        }

        // GET: Mission
        public ActionResult Index(string userName, MissionState? state, MissionType? type, int? page = 1)
        {
            Sidebar();
            var mission = db.Missions.Include(s => s.User).AsQueryable();
            //判断是否能申请升级
            var lvUpOneToTop = false;
            if (Comm.GetType(UserID) == Enums.User.Normal)
            {
                mission = mission.Where(s => s.UserID == UserID);
                var user = db.Users.Find(UserID);
                if (user.Rank == UserType.One)
                {
                    if (mission.Count(s => s.State == MissionState.CompleteNoCheck || s.State == MissionState.Start || s.State == MissionState.StartNoCheck) <= 0)
                    {
                        lvUpOneToTop = true;
                    }
                }
            }
            ViewBag.LvUpOneToTop = lvUpOneToTop;

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var user = db.Users.FirstOrDefault(s => s.UserName == userName || s.RealName == userName);
                user = user != null ? user : new ApplicationUser();
                mission.Where(s => s.UserID == user.Id);
            }
            if (state.HasValue)
            {
                mission = mission.Where(s => s.State == state.Value);
            }
            if (type.HasValue)
            {
                mission = mission.Where(s => s.Type == type.Value);
            }
            var userids = mission.Select(s => s.User.Recommend).Distinct();
            var users = db.Users.Where(s => userids.Contains(s.Id)).ToList();
            var list = mission.ToList()
                .Select(s => new MissionList(s, new ApplicationUser()))
                .OrderBy(s => s.CreateDateTime)
                .AsQueryable()
                .ToPagedList(page.Value);
            foreach (var item in list)
            {
                var recommend = users.FirstOrDefault(s => s.Id == item.User.Recommend);
                item.Recommend = recommend == null ? "公司" : recommend.RealName;
                item.RecommendPhone = recommend == null ? "" : recommend.PhoneNumber;
                if (item.State == MissionState.Start)
                {
                    if (item.MissionDetail.Last().StepID == 1)
                    {
                        item.MissionDetail.Last().JData = GetTeamSaleTotal(item.MissionDetail.Last()).ToString();
                    }
                    if (item.MissionDetail.Last().StepID == 2)
                    {
                        item.MissionDetail.Last().JData = GetRecommendCount(item.MissionDetail.Last()).ToString();
                    }
                }
            }
            return View(list);
        }

        // GET: Mission/Create
        public ActionResult Create()
        {
            Sidebar();
            var mission = db.Missions.Where(s => s.UserID == UserID &&
                (s.State == MissionState.CompleteNoCheck ||
                s.State == MissionState.Start ||
                s.State == MissionState.StartNoCheck
                ));
            if (mission.Count() > 0)
            {
                return RedirectToAction("Index");
            }
            var missionCompile = new MissionCompile()
            {
                Value = Bll.SystemSettings.Mission
            };
            return View(missionCompile);
        }

        // POST: Mission/Create
        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreateConfirm()
        {
            LvUpOneToTop lv = new LvUpOneToTop(UserID);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CancelMissionn(int id)
        {
            var mission = db.Missions.Find(id);
            if (mission.State != MissionState.Start)
            {
                return Json(Comm.ToMobileResult("Error", "任务不能中断"));
            }
            mission.State = MissionState.Failed;
            var last = mission.MissionDetail.Last();
            var md = new MissionDetail()
            {
                StepID = -(last.StepID + 1),
                UpdateTime = DateTime.Now,
                JData = "管理员中断任务",
            };
            mission.MissionDetail.Add(md);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "任务已中断"));
        }

        // GET: Mission/Edit/5
        [Authorize(Roles = SysRole.MissionManageCheck)]
        public ActionResult Edit(int id)
        {
            Sidebar();
            var mission = db.Missions.Find(id);
            var lv = new LvUpOneToTop(mission.UserID);
            return View(lv);
        }

        // POST: Mission/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, string select, string remark)
        {
            if (!User.IsInRole(SysRole.MissionManageCheck))
            {
                return Json(Comm.ToMobileResult("Error", "账号没有权限审核"));
            }
            var mission = db.Missions.Find(id);
            if (mission.State != MissionState.CompleteNoCheck && mission.State != MissionState.StartNoCheck)
            {
                return Json(Comm.ToMobileResult("Error", "任务不能审核"));
            }
            LvUpOneToTop lv = new LvUpOneToTop(mission.UserID);
            lv.Next(select, remark);
            return Json(Comm.ToMobileResult("Success", "审核完成"));
        }

        [HttpPost]
        public ActionResult MissionFinish(int id)
        {
            var mission = db.Missions.Include(s => s.User).FirstOrDefault(s => s.ID == id);
            MissionComplete(mission);
            return Json(Comm.ToMobileResult("Success", "完成任务"));
        }

        [HttpPost]
        public ActionResult Mission()
        {
            var missions = db.Missions.Where(s => s.State == MissionState.Start);
            foreach (var item in missions)
            {
                MissionComplete(item);
            }
            return Json(Comm.ToMobileResult("Success", "成功"));
        }

        public void MissionComplete(Mission model)
        {
            var last = model.MissionDetail.OrderBy(s => s.ID).Last();
            var lv = new LvUpOneToTop(last.Mission.UserID);
            if (last.StepID == 1)
            {
                var total = GetTeamSaleTotal(last);
                if (DateTime.Now <= last.UpdateTime.AddMonths(1) && total >= 800000)
                {
                    lv.Next("通过", "");
                }
                else if (DateTime.Now > last.UpdateTime.AddMonths(1) && total < 800000)
                {
                    lv.Next("不通过", "任务一失败");
                }
            }
            if (last.StepID == 2)
            {
                var count = GetRecommendCount(last);
                if (DateTime.Now <= last.UpdateTime.AddMonths(1))
                {
                    if (count >= 10)
                    {
                        lv.Next("通过", "");
                    }
                    else
                    {
                        lv.Next("不通过", "任务二失败");
                    }
                }
            }
        }

        /// <summary>
        /// 获取销量
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public decimal GetTeamSaleTotal(MissionDetail model)
        {
            var users = db.Users.Where(s => s.Recommend == model.Mission.UserID && s.Rank == model.Mission.User.Rank);
            var userids = users.Select(s => s.Id).ToList();
            userids.Add(model.Mission.UserID);
            var lastTime = model.UpdateTime.AddMonths(1);
            var orders = db.Orders.Where(s => s.CreateDateTime >= model.UpdateTime &&
                   s.CreateDateTime <= lastTime &&
                   s.ReceiptDateTime.HasValue &&
                   userids.Contains(s.UserID));
            decimal total = 0;
            foreach (var item in orders)
            {
                total = total + item.Total;
            }
            return total;
        }

        public int GetRecommendCount(MissionDetail model)
        {
            var lastTime = model.UpdateTime.AddMonths(1);
            var userProduct = db.Users.Where(s => s.RegisterDateTime >= model.UpdateTime &&
                  s.RegisterDateTime <= lastTime &&
                  s.Rank == UserType.One &&
                  s.Recommend == model.Mission.UserID);
            return userProduct.Count();
        }

    }
}
