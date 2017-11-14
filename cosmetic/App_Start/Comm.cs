using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cosmetic.Models;

namespace Cosmetic
{
    public class Comm
    {
        private static Random _random;
        /// <summary>
        /// 系统唯一随机
        /// </summary>
        public static Random Random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }

        /// <summary>
        /// 是否是移动端
        /// </summary>
        public static bool IsMobileDrive
        {
            get
            {
                var request = HttpContext.Current.Request;
                return request.Browser.IsMobileDevice || request.UserAgent.ToLower().Contains("micromessenger");
            }
        }

        /// <summary>
        /// 是否是移动端
        /// </summary>
        public static bool IsWeChat
        {
            get
            {
                var request = HttpContext.Current.Request;
                return request.UserAgent.ToLower().Contains("micromessenger");
            }
        }

        /// <summary>
        /// 设置WebConfig
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public static void SetConfig(string key, string val)
        {
            System.Configuration.ConfigurationManager.AppSettings.Set(key, val);
        }

        /// <summary>
        /// 读取WebConfig
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string key)
        {
            return (T)Convert.ChangeType(System.Configuration.ConfigurationManager.AppSettings[key], typeof(T));
        }

        ///// <summary>
        ///// 写LOG，LOG将按日期分类
        ///// </summary>
        ///// <param name="type">不同类别保存在不同文件里面</param>
        ///// <param name="message">正文</param>
        ///// <param name="url">请求地址</param>
        //public static void WriteLog(string type, string message, Enum.DebugLogLevel lv, string url = "", Exception ex = null)
        //{
        //    var sysDebugLog = GetConfig<Enum.DebugLog>("DebugLog");

        //    Action writeLog = () =>
        //    {
        //        var path = HttpContext.Current.Request.MapPath($"~/Logs/{DateTime.Now:yyyy-MM-dd}/{type}.log");
        //        System.IO.FileInfo info = new System.IO.FileInfo(path);
        //        if (!info.Directory.Exists)
        //        {
        //            info.Directory.Create();
        //        }
        //        System.IO.File.AppendAllText(path, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message} {url} {ex?.Source}");
        //    };

        //    switch (sysDebugLog)
        //    {
        //        case Enum.DebugLog.All:
        //            writeLog();
        //            break;
        //        default:
        //        case Enum.DebugLog.No:
        //            break;
        //        case Enum.DebugLog.Warning:
        //            if (lv == Enum.DebugLogLevel.Warning || lv == Enum.DebugLogLevel.Error)
        //            {
        //                writeLog();
        //            }
        //            break;
        //        case Enum.DebugLog.Error:
        //            if (lv == Enum.DebugLogLevel.Error)
        //            {
        //                writeLog();
        //            }
        //            break;
        //    }


        //}


        /// <summary>
        /// ResizeImage图片地址生成
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="w">最大宽度</param>
        /// <param name="h">最大高度</param>
        /// <param name="quality">质量0~100</param>
        /// <param name="image">占位图类别</param>
        /// <returns>地址为空返回null</returns>
        public static string ResizeImage(string url, int? w = null, int? h = null,
            int? quality = null,
            Enums.DummyImage? image = Enums.DummyImage.Default,
            Enums.ResizerMode? mode = null,
            Enums.ReszieScale? scale = null
            )
        {
            var Url = new System.Web.Mvc.UrlHelper(HttpContext.Current.Request.RequestContext);
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }
            else
            {
                var t = new Uri(HttpContext.Current.Request.Url, Url.Content(url)).AbsoluteUri;
                Dictionary<string, string> p = new Dictionary<string, string>();
                if (w.HasValue)
                {
                    p.Add("w", w.ToString());
                }
                if (h.HasValue)
                {
                    p.Add("h", h.ToString());
                }
                if (scale.HasValue)
                {
                    p.Add("scale", scale.Value.ToString());
                }
                if (quality.HasValue)
                {
                    p.Add("quality", quality.ToString());
                }
                if (image.HasValue)
                {
                    p.Add("404", image.ToString());
                }
                if (mode.HasValue)
                {
                    p.Add("mode", mode.ToString());
                }
                return t + p.ToParam("?");
            }
        }

        public static Dictionary<string, object> ToMobileResult(string state, string message, object data = null)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("CyState", state);
            result.Add("CyMessage", message);
            if (data != null)
            {
                foreach (var item in data.GetType().GetProperties())
                {
                    result.Add(item.Name, item.GetValue(data));
                }
            }

            return result;
        }

        public static Enums.User GetType(string Userid)
        {
            using (ApplicationDbContext db=new ApplicationDbContext())
            {
                var user = db.Users.FirstOrDefault(s => s.Id == Userid);
                return user.Type;
            }
        }

        //public static Dictionary<string, object> ToMobileResultForPagedList(PagedList.IPagedList page, object data = null, string head = null)
        //{
        //    if (!string.IsNullOrWhiteSpace(head))
        //    {

        //        return ToMobileResult("Success", "成功", new
        //        {
        //            Page = new
        //            {
        //                page.PageNumber,
        //                page.PageCount,
        //                page.HasNextPage
        //            },
        //            Data = data,
        //            Head = Bll.Head.GetHead(head)
        //        });
        //    }
        //    else
        //    {
        //        return ToMobileResult("Success", "成功", new
        //        {
        //            Page = new
        //            {
        //                page.PageNumber,
        //                page.PageCount,
        //                page.HasNextPage
        //            },
        //            Data = data
        //        });
        //    }

        //}


        //public static Enum.DriveType GetDriveType(HttpRequestBase request)
        //{
        //    string userAgent = HttpContext.Current.Request.UserAgent.ToLower();
        //    if (userAgent.Contains("windows phone"))
        //    {
        //        return Enum.DriveType.Windows;
        //    }
        //    if (userAgent.Contains("iphone;"))
        //    {
        //        return Enum.DriveType.IPhone;
        //    }
        //    if (userAgent.Contains("ipad;"))
        //    {
        //        return Enum.DriveType.IPad;
        //    }
        //    if (userAgent.Contains("android"))
        //    {
        //        return Enum.DriveType.Android;
        //    }
        //    return Enum.DriveType.Windows;
        //}


        public const string ExcelString = @"mso-number-format:\@";
    }
}