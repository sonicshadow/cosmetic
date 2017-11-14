using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace Cosmetic.Controllers
{
    [Authorize]
    public class SupplierProductController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        private void Sidebar()
        {
            ViewBag.Sidebar = "库存管理";
        }

        // GET: SupplierProduct
        [Authorize(Roles = SysRole.StockManageRead)]
        public ActionResult Index(int? pid, int? sid, int page = 1)
        {
            Sidebar();
            var sp = db.SupplierProducts.Include(s => s.Product).Include(s => s.Supplier);
            if (pid.HasValue)
            {
                sp = sp.Where(s => s.ProductID == pid);
            }
            if (sid.HasValue)
            {
                sp = sp.Where(s => s.SupplierID == sid);
            }
            GetPAndS();
            var model = sp.OrderBy(s => s.ID).ToPagedList(page);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetByPid(int pid)
        {
            if (pid == 0)
            {
                return Json(Comm.ToMobileResult("Error", "输入商品ID"));
            }
            var sp = db.SupplierProducts.Where(s => s.ProductID == pid).ToList();
            return Json(Comm.ToMobileResult("Success", "成功", new { Data = sp }));
        }

        [HttpPost]
        public ActionResult GetBySid(int sid)
        {
            if (sid == 0)
            {
                return Json(Comm.ToMobileResult("Error", "输入供应商ID"));
            }
            var sp = db.SupplierProducts.Where(s => s.SupplierID == sid).ToList();
            int count = 0;
            foreach (var item in sp)
            {
                count = count + item.Remaining;
            }
            return Json(Comm.ToMobileResult("Success", "成功", new { Data = count }));
        }

        [HttpPost]
        public ActionResult GetById(int id)
        {
            if (id == 0)
            {
                return Json(Comm.ToMobileResult("Error", "输入商品ID"));
            }
            var sp = db.SupplierProducts.FirstOrDefault(s => s.ID == id);
            if (sp == null)
            {
                return Json(Comm.ToMobileResult("Error", "没有这个"));
            }
            return Json(Comm.ToMobileResult("Success", "成功", new
            {
                data = new
                {
                    Number = sp.Number,
                    Remaining = sp.Remaining
                }
            }));
        }

        public void GetPAndS()
        {
            Sidebar();
            ViewBag.Product = db.Products
                .Select(s => new SelectListItem() { Text = s.Name, Value = s.ID.ToString() }).ToList();
            ViewBag.Supplier = db.Suppliers
                .Select(s => new SelectListItem() { Text = s.Name, Value = s.ID.ToString() }).ToList();
            ViewBag.Payee = Bll.Account.GetPayeeInSelect();
        }

        // GET: SupplierProduct/Create
        [Authorize(Roles = SysRole.StockManageCreate)]
        public ActionResult Create()
        {
            var model = new SupplierProduct() {
                Send=0,
                SendTotal=0
            };
            GetPAndS();
            return View(model);
        }
        
        // POST: SupplierProduct/Create
        [HttpPost]
        [Authorize(Roles = SysRole.StockManageCreate)]
        public ActionResult Create(SupplierProduct model, int PayeeID)
        {
            var result = true;
            if (model.ProductID == 0)
            {
                result = false;
                ModelState.AddModelError("ProductID", "请选择商品");
            }
            if (model.SupplierID == 0)
            {
                result = false;
                ModelState.AddModelError("SupplierID", "请选择供应商");
            }
            if (model.Price <= 0)
            {
                result = false;
                ModelState.AddModelError("Price", "请填写正确单价");
            }
            if (model.Count <= 0)
            {
                result = false;
                ModelState.AddModelError("Count", "请填写正确数量");
            }
            if (model.Send > model.Count)
            {
                result = false;
                ModelState.AddModelError("Send", "收货数量大于订单数量");
            }
            if (model.SendTotal > 0)
            {
                try
                {
                    Pay(model, 0, PayeeID);
                }
                catch (Exception ex)
                {
                    result = false;
                    ModelState.AddModelError("SendTotal", ex.Message);
                }
            }
            if (!result)
            {
                GetPAndS();
                return View(model);
            }
            var product = db.Products.FirstOrDefault(s => s.ID == model.ProductID);
            model.CreateTime = DateTime.Now;
            model.Remaining = model.Send;
            model.Number = product.No;
            model.Code = $"{DateTime.Now.ToString("yyyyMMdd")}{Comm.Random.Next(1000, 9999)}";
            db.SupplierProducts.Add(model);
            db.SaveChanges();
            if (model.Send > 0)
            {
                try
                {
                    addStock(model, 0);
                }
                catch (Exception)
                {
                   
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = SysRole.StockManageRead)]
        public ActionResult Details(int id)
        {
            Sidebar();
            var sp = db.SupplierProducts
                .Include(s => s.Product)
                .Include(s => s.Supplier)
                .FirstOrDefault(s => s.ID == id);
            var view = new SupplierProductViewModel(sp);
            if (view.IsSendTotal != 0)
            {
                GetPAndS();
            }
            return View(view);
        }

        [HttpPost]
        public ActionResult EditByCount(int id, int count)
        {
            if (!User.IsInRole(SysRole.StockManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", "用户没有权限修改"));
            }
            if (count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "填写收货数量"));
            }
            var sp = db.SupplierProducts.FirstOrDefault(s => s.ID == id);
            addStock(sp, count);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "收货成功"));
        }

        public void addStock(SupplierProduct sp, int count)
        {
            sp.Send = sp.Send + count;
            if (count == 0)
            {
                count = sp.Send;
            }
            sp.Remaining = sp.Remaining + count;
            if (sp.Send > sp.Count)
            {
                throw new Exception("收货数量大于订单数量");
            }
            sp.State = Enums.PurchaseOrderState.Ing;
            if (sp.SendTotal == sp.Total)
            {
                if (sp.Send == sp.Count)
                {
                    sp.State = Enums.PurchaseOrderState.Finish;
                }
            }
            //进库记录
            var stock = new Stock()
            {
                Count = count,
                CreateTime = DateTime.Now,
                ProductID = sp.ProductID,
                Type = Enums.StockType.AddStock,
                DataID = sp.ID,
                Remark = "进货",
            };
            db.Stock.Add(stock);
            db.SaveChanges();
        }

        [HttpPost]
        public ActionResult EditByPayee(int id, decimal price, int payeeID)
        {
            if (!User.IsInRole(SysRole.StockManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            if (price <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "填写支付金额"));
            }
            var sp = db.SupplierProducts.Include(s => s.Supplier).Include(s => s.Product).FirstOrDefault(s => s.ID == id);
            try
            {
                Pay(sp, price, payeeID);
                db.SaveChanges();
                return Json(Comm.ToMobileResult("Success", "付款成功"));
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Error", ex.Message));
            }
           
        }

        //添加购货资金变化
        public void Pay(SupplierProduct sp, decimal price, int payeeID)
        {
            if (!User.IsInRole(SysRole.StockManageEdit))
            {
                throw new Exception("用户没有权限修改");
            }
            sp.SendTotal = sp.SendTotal + price;
            if (price == 0)
            {
                price = sp.SendTotal;
            }
            //查看是否能够付款
            var payee = db.Payees.FirstOrDefault(s => s.ID == payeeID);
            var list = Bll.Account.GetPayeeAccount( payee.Bank, payee.BankCard);
            if (list < price)
            {
                throw new Exception($"付款账户金额不足,账户剩余{list}元");
            }
            if (sp.SendTotal > sp.Total)
            {
                throw new Exception("付款数大于订单总金额");
            }
            sp.State = Enums.PurchaseOrderState.Ing;
            if (sp.SendTotal == sp.Total)
            {
                if (sp.Send == sp.Count)
                {
                    sp.State = Enums.PurchaseOrderState.Finish;
                }
            }
            try
            {
                var account = new Account()
                {
                    Amount = price,
                    Remark = $"向{sp.Supplier.Name}购入商品",
                    Trader = sp.SupplierID.ToString(),
                    TraderType = Enums.TraderType.Supplier,
                    UserType = null,
                    Fee = 0,
                    UpdateDateTime=DateTime.Now,
                };
                Bll.Account.Create(account, "商品类支出", payeeID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // POST: SupplierProduct/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Enums.StockType type, string remark, int count)
        {
            if (!User.IsInRole(SysRole.StockManageEdit))
            {
                return Json(Comm.ToMobileResult("Error", $"用户没有权限修改"));
            }
            if (count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "填写数量"));
            }
            var sp = db.SupplierProducts.FirstOrDefault(s => s.ID == id);
            if (type == Enums.StockType.Loss)
            {
                sp.Remaining = sp.Remaining - count;
                if (sp.Remaining < 0)
                {
                    return Json(Comm.ToMobileResult("Error", "已经没有库存，不能盘亏"));
                }
            }
            if (type == Enums.StockType.Overage)
            {
                sp.Remaining = sp.Remaining + count;
            }
            var stock = new Stock()
            {
                Count = type == Enums.StockType.Loss ? -count : count,
                CreateTime = DateTime.Now,
                ProductID = sp.ProductID,
                DataID = id,
                Type = type,
                Remark = remark,
            };
            db.Stock.Add(stock);
            db.SaveChanges();
            return Json(Comm.ToMobileResult("Success", "成功"));
        }

    }
}
