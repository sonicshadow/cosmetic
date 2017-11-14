using Cosmetic.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class DeliverHistoryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private string UserID
        {
            get { return User.Identity.GetUserId(); }
        }
        private void Sidebar()
        {
            ViewBag.Sidebar = "发货审核";
        }

        // POST: DeliverHistory/Create
        [HttpPost]
        public ActionResult Create(DeliverHistory deliver, SupplierProduct sp)
        {
            if (string.IsNullOrWhiteSpace(deliver.Address))
            {
                return Json(Comm.ToMobileResult("Error", "填写发货地址"));
            }
            if (string.IsNullOrWhiteSpace(deliver.Code))
            {
                return Json(Comm.ToMobileResult("Error", "填写快递号"));
            }
            if (string.IsNullOrWhiteSpace(deliver.Express))
            {
                return Json(Comm.ToMobileResult("Error", "填写快递名称"));
            }
            if (deliver.Count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "填写发货数量"));
            }
            //创建物流
            deliver.CreateDateTime = DateTime.Now;
            //扣订单的数量
            var order = db.Orders.Include(s => s.User).FirstOrDefault(s => s.ID == deliver.OrderID);
            var deliverHistories = order.DeliverHistory.Where(s => s.CheckState == Enums.CheckState.NoCheck);
            if (order.Count <= order.Send + deliverHistories.Sum(s => s.Count))
            {
                return Json(Comm.ToMobileResult("Error", "发货已经提交审核了，请勿再提交"));
            }
            if (order.Count < order.Send + deliverHistories.Sum(s => s.Count) + deliver.Count)
            {
                return Json(Comm.ToMobileResult("Error", "发货数量大于订单数量了"));
            }
            if (!string.IsNullOrWhiteSpace(order.ParentUser))
            {
                try
                {
                    deliver.CheckState = Enums.CheckState.Pass;
                    deliver.CheckTime = DateTime.Now;
                    deliver.DataID = 0;
                    db.DeliverHistories.Add(deliver);
                    RealDeliver(deliver, order);
                    db.SaveChanges();
                    return Json(Comm.ToMobileResult("Success", "发货成功"));
                }
                catch (Exception ex)
                {
                    return Json(Comm.ToMobileResult("Error", ex.Message));
                }
            }
            else
            {
                deliver.CheckState = Enums.CheckState.NoCheck;
                deliver.DataID = sp.SupplierID;
                db.DeliverHistories.Add(deliver);
                db.SaveChanges();
                return Json(Comm.ToMobileResult("Success", "发货操作已提交审核"));
            }
        }


        public ActionResult Check(Enums.CheckState? checkState, int page = 1)
        {
            Sidebar();
            var deliverHistory = db.DeliverHistories.Include(s => s.Order)
                .Include(s => s.Order.User)
                .Where(s => s.Order.ParentUser == null);
            ViewBag.NoCheckCount = deliverHistory.ToList().Count(s => s.CheckState == Enums.CheckState.NoCheck);
            if (checkState.HasValue)
            {
                deliverHistory = deliverHistory.Where(s => s.CheckState == checkState.Value);
            }
            var paged = deliverHistory.OrderBy(s => s.CreateDateTime).ToPagedList(page);
            var userids = paged.Select(s => s.CheckUser).Distinct();
            var users = db.Users.Where(s => UserID.Contains(s.Id)).ToList();
            foreach (var item in paged)
            {
                var user = users.FirstOrDefault(s => s.Id == item.CheckUser);
                item.CheckUser = user == null ? "" : user.UserName;
            }
            return View(paged);
        }

        [HttpPost]
        public ActionResult CheckDeliver(int id, bool result)
        {
            if (!User.IsInRole(SysRole.DeliverCheck))
            {
                return Json(Comm.ToMobileResult("Error", "没有权限审核"));
            }
            var deliver = db.DeliverHistories.Include(s => s.Order.User).FirstOrDefault(s => s.ID == id);
            if (deliver == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有这个审核记录"));
            }
            if (deliver.CheckState != Enums.CheckState.NoCheck)
            {
                return Json(Comm.ToMobileResult("Error", "记录已完成审核"));
            }
            deliver.CheckTime = DateTime.Now;
            deliver.CheckUser = UserID;
            if (result)
            {
                deliver.CheckState = Enums.CheckState.Pass;
                try
                {
                    RealDeliver(deliver, deliver.Order);
                }
                catch (Exception ex)
                {
                    return Json(Comm.ToMobileResult("Error", ex.Message));
                }
            }
            else
            {
                deliver.CheckState = Enums.CheckState.NoPass;
            }
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "审核完成"));
        }

        public void RealDeliver(DeliverHistory deliver, Order order)
        {
            //扣订单的数量
            order.Send = order.Send + deliver.Count;
            order.State = order.Count == order.Send ? Enums.OrderState.Finish : Enums.OrderState.Send;
            if (order.State == Enums.OrderState.Finish &&!string.IsNullOrWhiteSpace(order.ParentUser))
            {
                Bll.UserIncome.CreateUserIncome(order);
            }
            if (order.Send > order.Count)
            {
                throw new Exception("发货数量大于订单数量了");
            }
            //添加库存
            var userStock = db.UserProducts.FirstOrDefault(s => s.UserID == order.UserID && s.ProductID == order.ProductID);
            userStock.Sum = userStock.Sum + deliver.Count;
            userStock.Count = userStock.Count + deliver.Count;
            if (order.User.Rank == Enums.UserType.Retailer)
            {
                userStock.Count = userStock.Count - deliver.Count;
                var RetailerStock = new Stock()
                {
                    CreateTime = DateTime.Now,
                    Count = -deliver.Count,
                    ProductID = order.ProductID,
                    Type = Enums.StockType.Use,
                    UserID = order.UserID,
                    DataID =0,
                    Remark= "自己做脸使用",
                };
                db.Stock.Add(RetailerStock);
            }
            //出库记录
            var stock = new Stock()
            {
                CreateTime = DateTime.Now,
                Count = -deliver.Count,
                ProductID = order.ProductID,
                Type = Enums.StockType.Deliver,
                UserID = order.UserID,
                DataID = order.ID,
            };
            if (string.IsNullOrWhiteSpace(order.ParentUser))
            {
                //如果上级发货人是公司 扣实际库存
                var supplierProducts = db.SupplierProducts.Where(s => s.ProductID == order.ProductID && s.SupplierID == deliver.DataID);
                var send = deliver.Count;
                foreach (var item in supplierProducts)
                {
                    if (item.Remaining > 0)
                    {
                        if (item.Remaining >= send)
                        {
                            item.Remaining = item.Remaining - send;
                            send = 0;
                        }
                        else
                        {
                            send = send - item.Remaining;
                            item.Remaining = 0;
                        }
                    }
                }
                if (send > 0)
                {
                    throw new Exception("库存不足，请添加库存");
                }
                stock.Remark = $"公司出货给{order.User.RealName}";
            }
            else
            {
                //上级发货人不是公司的话   扣上级发货人的库存                
                var parents = db.UserProducts.Include(s => s.User).FirstOrDefault(s => s.ProductID == order.ProductID && s.UserID == UserID);
                parents.Sent = parents.Sent + deliver.Count;
                parents.Count = parents.Count - deliver.Count;
                if (parents.Count < 0)
                {
                    throw new Exception("库存不足，请添加库存");
                }
                stock.Remark = $"{parents.User.RealName}出货给{order.User.RealName}";
            }
            db.Stock.Add(stock);
            //发出通知
            ISms sms = new CnSms();  
            var product = db.Products.FirstOrDefault(s => s.ID == order.ProductID);
            var result = sms.Send(order.User.PhoneNumber, $"尊敬的汉焱祖方用户，你的订单号{order.Code}，《{product.Name}》已发货{deliver.Count}套，请注意查收.{deliver.Express}快递或物流，单号{deliver.Code}");
            if (!result.IsSuccess)
            {
                throw new Exception(result.Message);
            }
            db.SaveChanges();
        }
    }
}
