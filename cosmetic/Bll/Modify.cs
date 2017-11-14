using Cosmetic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosmetic.Bll
{
    public class Modify
    {
        public static void Create(Models.Modify model)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var m = new Models.Modify()
                {
                    ModifyType=model.ModifyType,
                    NewData=model.NewData,
                    OldData=model.OldData,
                    Time=DateTime.Now,
                    UserID=model.UserID,
                };
                db.Modifys.Add(m);
                db.SaveChanges();
            }
        }
    }
}