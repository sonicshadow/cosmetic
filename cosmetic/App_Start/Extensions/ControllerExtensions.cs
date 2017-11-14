﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity;
using Cosmetic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cosmetic
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// 直接调用框架自带HttpStatus只能返回ASCII，不能显示英文数字意外的字符
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="code">错误代码</param>
        /// <param name="message">消息</param>
        /// <returns></returns>
        public static ActionResult HttpStatusCodeResultUTF8(this Controller controller, System.Net.HttpStatusCode code, string message)
        {
            controller.Response.StatusCode = (int)code;

            //Needed for IIS7.0
            controller.Response.TrySkipIisCustomErrors = true;

            return new ContentResult
            {
                Content = message,
                ContentEncoding = System.Text.Encoding.UTF8
            };
        }

        /// <summary>
        /// 跳转到错误页面
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="head"></param>
        /// <param name="message"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public static ActionResult ToError(this Controller controller, string head, string message, string returnUrl = null)
        {
            var view = new ViewResult();
            view.ViewName = "Error";
            view.ViewBag.Head = string.IsNullOrWhiteSpace(head) ? "错误" : head;
            view.ViewBag.Message = message + (string.IsNullOrWhiteSpace(returnUrl) ? "" : $"<a href='{controller.Url.Content(returnUrl)}'>点击返回</a>");
            return view;
        }




        /// <summary>
        /// 导出Excel时候输出头
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="fileName"></param>
        public static void AddExcelExportHead(this Controller controller, string fileName)
        {
            controller.Response.Buffer = true;
            controller.Response.AddHeader("content-disposition", $"attachment; filename={fileName}.xls");
            controller.Response.ContentType = "application/ms-excel";
            controller.Response.ContentEncoding = System.Text.Encoding.UTF8;
        }

        /// <summary>
        /// 导出Excel时候输出头
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="fileName"></param>
        public static void AddWordExportHead(this Controller controller, string fileName)
        {
            controller.Response.Buffer = true;
            controller.Response.AddHeader("content-disposition", $"attachment; filename={fileName}.doc");
            controller.Response.ContentType = "application/msword";
            controller.Response.ContentEncoding = System.Text.Encoding.UTF8;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="path">路径</param>
        /// <param name="requestName">目标的Request.Name,为空时候全部上传</param>
        /// <returns></returns>
        public static ICollection<string> UploadFile(this Controller controller, string path = "~/Upload",
            string requestName = null, bool isResetName = false)
        {
            var Server = controller.Server;
            var Request = controller.Request;
            string dir = controller.Server.MapPath(path);

            string[] images = new string[Request.Files.Count];
            if (Request.Files.Count == 0 || Request.Files[0].ContentLength == 0)
            {
                return new string[0];
            }
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            List<string> fileNames = new List<string>();
            Action<HttpPostedFileBase> saveFile = item =>
            {
                FileInfo file = new FileInfo(item.FileName);
                string filePath;
                string name = $"{path}/{DateTime.Now:yyyyMMddHHmmss}{Comm.Random.Next(1000, 9999)}{file.Extension}";

                do
                {

                    if (isResetName)
                    {
                        name = $"{path}/{file.Name}";
                    }
                    fileNames.Add(name);
                    filePath = Server.MapPath(name);
                } while (File.Exists(filePath));
                item.SaveAs(filePath);
                var cloudFile = new FileInfo(Server.MapPath(name));
               
            };
            if (string.IsNullOrWhiteSpace(requestName))
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    saveFile(Request.Files[i]);
                }
            }
            else
            {
                if (Request.Files[requestName] == null)
                {
                    throw new Exception($"文件{requestName}不存在");
                }
                saveFile(Request.Files[requestName]);
            }

            return fileNames;
        }

        /// <summary>
        /// 是否是移动端的请求
        /// </summary>
        public static bool IsMobileDrive
        {
            get
            {
                return Comm.IsMobileDrive;
            }
        }
    }
}