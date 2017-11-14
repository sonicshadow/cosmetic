using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cosmetic.Models
{
    public class Mission
    {
        public int ID { get; set; }

        [Display(Name = "用户")]
        public string UserID { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "关联数据ID")]
        public int DataID { get; set; }

        [Display(Name = "状态")]
        public Enums.MissionState State { get; set; }

        [Display(Name = "类别")]
        public Enums.MissionType Type { get; set; }

        [Display(Name = "创建时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateDateTime { get; set; }

        public virtual List<MissionDetail> MissionDetail { get; set; }

    }

    public class MissionDetail
    {
        public int ID { get; set; }

        [Display(Name = "步骤ID")]
        public int StepID { get; set; }

        [Display(Name = "任务ID")]
        public int MissionID { get; set; }
        public virtual Mission Mission { get; set; }

        [Display(Name = "时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// Json数据存放
        /// </summary>
        [Display(Name = "备注")]
        public string JData { get; set; }
    }

    /// <summary>
    /// 流程分支
    /// </summary>
    public class MissionSeletor
    {
        public MissionSeletor(string name, int next)
        {
            Name = name;
            Next = next;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下一个模块的ID
        /// </summary>
        public int Next { get; set; }


    }

    /// <summary>
    /// 流程模块
    /// </summary>
    public class MissionItem
    {
        public MissionItem(int itemId, string name)
        {
            ItemID = itemId;
            Name = name;
            Selector = new List<MissionSeletor>();
        }
        public MissionItem()
        {
        }
        /// <summary>
        /// ID（必须唯一）
        /// </summary>
        public int ItemID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 流程分支，如果结束接点分支不要添加
        /// </summary>
        public List<MissionSeletor> Selector { get; set; } = new List<MissionSeletor>();

    }

    /// <summary>
    /// 流程图基类
    /// </summary>
    public abstract class BaseFlow : IDisposable
    {
        protected ApplicationDbContext db = new ApplicationDbContext();
        public virtual void Init(string userId)
        {
            Mission = db.Missions.Include(s => s.User).Where(s => s.Type == Type &&
                      s.UserID == userId &&
                      s.State != Enums.MissionState.CompleteNotPass &&
                      s.State != Enums.MissionState.StartNotPass &&
                      s.State != Enums.MissionState.Failed)
                    .OrderBy(s => s.CreateDateTime).FirstOrDefault();
            if (Mission == null)
            {
                Mission = new Mission()
                {
                    CreateDateTime = DateTime.Now,
                    State = Enums.MissionState.StartNoCheck,
                    Type = Type,
                    UserID = userId,
                };
                db.Missions.Add(Mission);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 当前流程（数据库）
        /// </summary>
        public Mission Mission { get; set; }

        /// <summary>
        /// 最新流程明细（数据库）
        /// </summary>
        public MissionDetail Last { get; set; }

        /// <summary>
        /// 类别（继承后必须重新）
        /// </summary>
        public abstract Enums.MissionType Type { get; }

        /// <summary>
        /// 流程当前的处理
        /// </summary>
        public MissionItem Item { get; set; } = new MissionItem();

        /// <summary>
        /// 所有处理过程（整个流程图）
        /// </summary>
        public List<MissionItem> Items { get; set; } = new List<MissionItem>();

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="name">选择器名称，流程分支名称</param>
        /// <param name="remark">备注</param>
        public virtual void Next<T>(string name, T remark)
        {
            var selector = Item.Selector.FirstOrDefault(s => s.Name == name);
            if (typeof(T) == typeof(string))
            {
                Last.JData = (string)Convert.ChangeType(remark, typeof(string));
            }
            else
            {
                Last.JData = JsonConvert.SerializeObject(remark);
            }
        }

        /// <summary>
        /// 析构
        /// </summary>
        public void Dispose()
        {
            db.Dispose();
        }
    }

    public class LvUpOneToTop : BaseFlow
    {
        public LvUpOneToTop(string userId)
        {
            Init(userId);
            if (Mission.MissionDetail == null || Mission.MissionDetail.Count <= 0)
            {
                var detail = new MissionDetail
                {
                    MissionID = Mission.ID,
                    StepID = 0,
                    UpdateTime = DateTime.Now,
                };
                db.MissionDetails.Add(detail);
                db.SaveChanges();
                Last = detail;
            }
            else
            {
                Last = Mission.MissionDetail.OrderBy(s => s.ID).Last();
            }
            SetItems();
            Item = Items.FirstOrDefault(s => s.ItemID == Last.StepID);
        }

        //初始化步骤
        private void SetItems()
        {
            var start = new MissionItem(0, "提交申请");
            start.Selector.Add(new MissionSeletor("不通过", -1));
            start.Selector.Add(new MissionSeletor("通过", 1));
            Items.Add(start);

            var statrNoPass = new MissionItem(-1, "申请不通过");
            Items.Add(statrNoPass);

            var mission1 = new MissionItem(1, "开始第一个任务");
            mission1.Selector.Add(new MissionSeletor("不通过", -2));
            mission1.Selector.Add(new MissionSeletor("通过", 2));
            Items.Add(mission1);

            var mission1NotPass = new MissionItem(-2, "任务1未完成");
            Items.Add(mission1NotPass);

            var mission2 = new MissionItem(2, "开始第二个任务");
            mission2.Selector.Add(new MissionSeletor("不通过", -3));
            mission2.Selector.Add(new MissionSeletor("通过", 3));
            Items.Add(mission2);

            var mission2NotPass = new MissionItem(-3, "任务2未完成");
            Items.Add(mission2NotPass);

            var complete = new MissionItem(3, "升级审核");
            complete.Selector.Add(new MissionSeletor("不通过", -4));
            complete.Selector.Add(new MissionSeletor("通过", 4));
            Items.Add(complete);

            var completeNotPass = new MissionItem(-4, "升级审核不通过");
            Items.Add(completeNotPass);

            var completePass = new MissionItem(4, "升级完成");
            Items.Add(completePass);
        }

        public override void Next<T>(string name, T remark)
        {
            base.Next<T>(name, remark);
            var selector = Item.Selector.FirstOrDefault(s => s.Name == name);
            var next = Items.FirstOrDefault(s => s.ItemID == selector.Next);
            var jData = "";
            if (typeof(T) == typeof(string))
            {
                jData = (string)Convert.ChangeType(remark, typeof(string));
            }
            else
            {
                jData = JsonConvert.SerializeObject(remark);
            }
            var details = new MissionDetail()
            {
                MissionID = Mission.ID,
                StepID = next.ItemID,
                UpdateTime = DateTime.Now,
                JData = jData
            };

            if (next.ItemID == -1)
            {
                Mission.State = Enums.MissionState.StartNotPass;
            }
            if (next.ItemID == -2 || next.ItemID == -3)
            {
                Mission.State = Enums.MissionState.Failed;
            }
            if (next.ItemID == -4)
            {
                Mission.State = Enums.MissionState.CompleteNotPass;
            }
            if (next.ItemID == 1 || next.ItemID == 2)
            {
                var time = DateTime.Now;
                if (next.ItemID == 2)
                {
                    if (DateTime.Now > Last.UpdateTime.AddMonths(1))
                    {
                        time = Last.UpdateTime.AddMonths(1);
                    }
                }
                details.UpdateTime = time;
                Mission.State = Enums.MissionState.Start;
            }
            if (next.ItemID == 3)
            {
                details.UpdateTime = Last.UpdateTime;
                Mission.State = Enums.MissionState.CompleteNoCheck;
            }
            if (next.ItemID == 4)
            {
                details.UpdateTime = DateTime.Now;
                //任务完成
                Mission.State = Enums.MissionState.CompletePass;
                //改用户等级和商品价格
                var user = db.Users.FirstOrDefault(s => s.Id == Mission.UserID);
                user.Rank = Enums.UserType.Premium;
                var ups = db.UserProducts.Where(s => s.UserID == user.Id).ToList();
                var productDetail = db.ProductDetails.Where(s => s.UseType == Enums.UserType.Premium).ToList();
                foreach (var item in ups)
                {
                    var de = productDetail.FirstOrDefault(s => s.ProductID == item.ProductID);
                    item.Min = de.Min;
                    item.TwiceMin = de.TwiceMin;
                    item.Price = de.Price;
                }
                //额外任务
                var lastTime = Last.UpdateTime.AddMonths(1);
                var userProduct = db.Users.Where(s => s.RegisterDateTime >= Last.UpdateTime &&
                      s.RegisterDateTime <= lastTime &&
                      s.Rank == Enums.UserType.One &&
                      s.Recommend == Last.Mission.UserID);
                if (userProduct.Count() >= 20)
                {
                    var userIncome = new UserIncome()
                    {
                        Amount = 250000,
                        CreateDateTime = DateTime.Now,
                        Type = Enums.UserIncomeType.Mission,
                        UserID = Mission.UserID,
                        IsPay = false,
                    };
                    db.UserIncomes.Add(userIncome);
                }
            }
            db.MissionDetails.Add(details);
            db.SaveChanges();
        }

        public override Enums.MissionType Type { get; } = Enums.MissionType.LvUpOneToTop;
    }


    [NotMapped]
    public class MissionList : Mission
    {
        public MissionList() { }

        public MissionList(Mission mission, ApplicationUser recommend)
        {
            ID = mission.ID;
            CreateDateTime = mission.CreateDateTime;
            DataID = mission.DataID;
            MissionDetail = mission.MissionDetail;
            State = mission.State;
            Type = mission.Type;
            User = mission.User;
            UserID = mission.UserID;
            Recommend = recommend == null ? "公司" : recommend.RealName;
            RecommendPhone = recommend == null ? "" : recommend.PhoneNumber;
        }

        /// <summary>
        /// 推荐人
        /// </summary>
        [Display(Name = "推荐人")]
        public string Recommend { get; set; }

        /// <summary>
        /// 推荐人手机号
        /// </summary>
        [Display(Name = "推荐人手机号")]
        public string RecommendPhone { get; set; }

    }


    public class Receivables : BaseFlow
    {
        public Receivables(int orderID, string userid, Account account)
        {
            Mission = db.Missions.Where(s => s.Type == Type && s.DataID == orderID &&
                    s.State == Enums.MissionState.CompleteNoCheck)
                .OrderBy(s => s.CreateDateTime).FirstOrDefault();
            if (Mission == null)
            {
                Mission = new Mission()
                {
                    CreateDateTime = DateTime.Now,
                    State = Enums.MissionState.CompleteNoCheck,
                    Type = Type,
                    UserID=userid,
                    DataID = orderID
                };
                db.Missions.Add(Mission);
                db.SaveChanges();
            }
            if (Mission.MissionDetail == null || Mission.MissionDetail.Count <= 0)
            {
                var detail = new MissionDetail
                {
                    MissionID = Mission.ID,
                    StepID = 0,
                    UpdateTime = DateTime.Now,
                    JData = JsonConvert.SerializeObject(account),
                };
                db.MissionDetails.Add(detail);
                db.SaveChanges();
                Last = detail;
            }
            else
            {
                Last = Mission.MissionDetail.OrderBy(s => s.ID).Last();
            }
            SetItems();
            Item = Items.FirstOrDefault(s => s.ItemID == Last.StepID);
        }

        //初始化步骤
        private void SetItems()
        {
            var start = new MissionItem(0, "申请审核");
            start.Selector.Add(new MissionSeletor("不通过", -1));
            start.Selector.Add(new MissionSeletor("通过", 1));
            Items.Add(start);

            var statrNoPass = new MissionItem(-1, "审核不通过");
            Items.Add(statrNoPass);

            var mission1 = new MissionItem(1, "审核通过");
            Items.Add(mission1);

        }

        public override void Next<T>(string name, T remark)
        {
            var selector = Item.Selector.FirstOrDefault(s => s.Name == name);
            var next = Items.FirstOrDefault(s => s.ItemID == selector.Next);
            var jData = "";
            if (typeof(T) == typeof(string))
            {
                jData = (string)Convert.ChangeType(remark, typeof(string));
            }
            else
            {
                jData = JsonConvert.SerializeObject(remark);
            }
            var details = new MissionDetail()
            {
                MissionID = Mission.ID,
                StepID = next.ItemID,
                UpdateTime = DateTime.Now,
                JData = jData,
            };
            if (next.ItemID == -1)
            {
                Mission.State = Enums.MissionState.CompleteNotPass;
            }
            if (next.ItemID == 1)
            {
                //任务完成
                Mission.State = Enums.MissionState.CompletePass;
                var data = Mission.MissionDetail.FirstOrDefault(s => s.StepID == 0).JData;
                var account = JsonConvert.DeserializeObject<Account>(data);
                //修改订单信息
                var order = db.Orders.FirstOrDefault(s => s.ID == Mission.DataID);               
                order.State = Enums.OrderState.Pay;
                order.ReceiptDateTime = account.UpdateDateTime;
                order.BankAccount = account.BankAccount;
                order.BankCard = account.BankCard;
                order.BankName = account.BankName;
                order.State = Enums.OrderState.Pay;
                if (string.IsNullOrWhiteSpace(order.ParentUser))
                {
                    Bll.UserIncome.CreateUserIncome(order);
                }
                var userProduct = db.UserProducts
                   .FirstOrDefault(s => s.UserID == order.UserID && s.ProductID == order.ProductID);
                userProduct.Sum = userProduct.Sum + order.Count;
                //添加资金
                db.Accounts.Add(account);
            }
            db.MissionDetails.Add(details);
            db.SaveChanges();
        }

        public override Enums.MissionType Type { get; } = Enums.MissionType.Receivables;
    }

    [NotMapped]
    public class ReceivablesList : Mission
    {
        public ReceivablesList() { }

        public ReceivablesList(Mission mission,Order o)
        {
            ID = mission.ID;
            CreateDateTime = mission.CreateDateTime;
            DataID = mission.DataID;
            MissionDetail = mission.MissionDetail;
            State = mission.State;
            Type = mission.Type;
            User = mission.User;
            UserID = mission.UserID;
            Account = JsonConvert.DeserializeObject<Account>(mission.MissionDetail.First().JData);
            Order = o;
        }

        public Account Account { get; set; }

        public Order Order { get; set; }

    }

}