using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cosmetic.Models;
using Microsoft.AspNet.Identity;
using System.IO;

namespace Cosmetic.Controllers
{
    public class UploaderController : Controller
    {
        public string UserID
        {
            get
            {
                return User.Identity.GetUserId();
            }
        }

        [HttpPost]
        public ActionResult Upload(FileUpload model)
        {
            ICollection<string> filename = null;
            if (string.IsNullOrWhiteSpace(model.FilePath))
            {
                filename = this.UploadFile(isResetName: model.IsResetName);
            }
            else
            {
                filename = this.UploadFile(path: model.FilePath, isResetName: model.IsResetName);
            }
            if (filename == null || filename.Count <= 0)
            {
                return Json(Comm.ToMobileResult("Error", "上传失败"));
            }
            return Json(Comm.ToMobileResult("Success", "成功", new
            {
                FileUrls = filename,
                FileFullUrls = filename.Select(s => Url.ContentFull(s))
            }));
        }

        public ActionResult DeleteFile(string file)
        {
            try
            {
                DeleteSeverFile(file);
                return Json(Comm.ToMobileResult("Success", "删除成功"));
            }
            catch (Exception ex)
            {
                return Json(Comm.ToMobileResult("Error", "删除失败"));
            }
        }

        private void DeleteSeverFile(string file)
        {
            var fileName = file.ToLower();
            if (!fileName.Contains("~/upload/"))
            {
                throw new Exception("Error");
            }
            file = Server.MapPath(file);
            try
            {
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                {
                    throw new DirectoryNotFoundException("文件不存在");
                }
                System.IO.File.Delete(file);
            }
            catch (System.IO.IOException e)
            {
                throw e;
            }
        }



        //[HttpPost]
        //[Authorize]
        //public ActionResult DeleteCloudFile(int[] ids)
        //{
        //    using (ApplicationDbContext db = new ApplicationDbContext())
        //    {
        //        var cloudFile = db.CloudFiles
        //            .Where(s => ids.Contains(s.ID) && s.UserID == UserID)
        //            .ToList();
        //        if (cloudFile.Count > 0)
        //        {
        //            foreach (var item in cloudFile)
        //            {
        //                try
        //                {
        //                    DeleteSeverFile(item.Url);
        //                    db.CloudFiles.Remove(item);
        //                }
        //                catch (DirectoryNotFoundException)
        //                {
        //                    db.CloudFiles.Remove(item);
        //                }
        //                catch (Exception ex)
        //                {

        //                }
        //            }
        //            db.SaveChanges();
        //            return Json(Comm.ToMobileResult("Success", "删除成功"));
        //        }
        //        else
        //        {
        //            return Json(Comm.ToMobileResult("FileNoFound", "文件不存在"));
        //        }
        //    }

        //}
        //[HttpPost]
        //[Authorize]
        //public ActionResult UploadByCloud()
        //{
        //    var filenames = this.UploadFile().ToList();
        //    if (filenames.Count > 0 && !string.IsNullOrWhiteSpace(UserID))
        //    {
        //        var file = Request.Files[0];
        //        CloudFileViewModel result;
        //        using (ApplicationDbContext db = new ApplicationDbContext())
        //        {
        //            CloudFile cFile = new CloudFile(UserID, file, filenames[0]);
        //            db.CloudFiles.Add(cFile);
        //            db.SaveChanges();
        //            result = new CloudFileViewModel(cFile);
        //        };
        //        return Json(Comm.ToMobileResult("Success", "成功", result));
        //    }
        //    return Json(Comm.ToMobileResult("Error", "失败"));
        //}

        [HttpPost]
        public ActionResult ForCkEditor()
        {
            var filename = this.UploadFile().ToList();
            if (filename == null || filename.Count <= 0)
            {
                return Json(new { State = "Error", Message = "未找到上传文件" });
            }
            ViewBag.CKEditorFuncNum = Request["CKEditorFuncNum"];
            var name = $"{filename[0]}?404=default";
            ViewBag.File = Url.ContentNullEmpty(name);
            if (Request["responseType"] == "json")
            {
                return Json(new { fileName = new System.IO.FileInfo(filename[0]).Name, uploaded = 1, url = Url.Content(name) });
            }
            else
            {
                return View();
            }
        }
    }
}