using System;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Cosmetic
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui/js").Include(
                  "~/Scripts/jquery-ui-1.11.4.custom/jquery-ui.min.js"));
            bundles.Add(new StyleBundle("~/bundles/jqueryui/css").Include(
                    "~/Scripts/jquery-ui-1.11.4.custom/jquery-ui.min.css"));

            //公共js
            bundles.Add(new ScriptBundle("~/bundles/comm").Include(
                        "~/Scripts/Comm/canvas-to-blob.min.js",
                        "~/Scripts/Comm/jsEx.js",
                        "~/Scripts/Comm/jQueryEx.js",
                        "~/Scripts/Comm/comm.js",
                        "~/Scripts/Comm/check.js",
                        "~/Scripts/Comm/searchUser.js",
                        "~/Scripts/Comm/uploadfile.js",
                        "~/Scripts/Comm/imageResizeUpload.js",
                        "~/Scripts/Comm/jquery.dotdotdot.min.js"
                    ));


            //日期控件
            bundles.Add(new StyleBundle("~/bundles/datetimepicker/css").Include(
                  "~/Scripts/datatimepicker/bootstrap-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/datetimepicker/js").Include(
                    "~/Scripts/datatimepicker/bootstrap-datetimepicker.js",
                    "~/Scripts/datatimepicker/locales/bootstrap-datetimepicker.zh-CN.js"));

            //swiper
            bundles.Add(new StyleBundle("~/bundles/swiper/css").Include(
                       "~/Scripts/Swiper/css/swiper.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/swiper/js").Include(
                "~/Scripts/Swiper/js/swiper.jquery.min.js",
                "~/Scripts/Swiper/js/swiper.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/cloud").Include(
                   "~/Scripts/Comm/cloud.js"
               ));

            //view js
            Action<string, string[]> addViewScripts = (name, js) =>
            {
                js = js.Select(s => $"~/Scripts/Views/{s}").ToArray();
                bundles.Add(new ScriptBundle($"~/bundles/{name}").Include(js));
            };

            addViewScripts("staffsIndex", new string[] { "staffsIndex.js" });
            addViewScripts("accountManageIndex", new string[] { "accountManageIndex.js" });
            addViewScripts("roleGroup", new string[] { "roleGroup.js" });
            addViewScripts("bankIndex", new string[] { "bankIndex.js" });
            addViewScripts("userProductTeam", new string[] { "userProductTeam.js" });
            addViewScripts("register", new string[] { "register.js" });
            addViewScripts("supplierProductIndex", new string[] { "supplierProductIndex.js" });
            addViewScripts("missionIndex", new string[] { "missionIndex.js" });
            addViewScripts("userIndex", new string[] { "userIndex.js" });
            addViewScripts("productCreateEdit", new string[] { "productCreateEdit.js" });
            addViewScripts("userEdit", new string[] { "userEdit.js" });
            addViewScripts("userIncomeIndex", new string[] { "userIncomeIndex.js" });
            addViewScripts("orderEdit", new string[] { "orderEdit.js" });
            addViewScripts("notice", new string[] { "notice.js" });
            addViewScripts("displayManage", new string[] { "displayManage.js" });
            addViewScripts("holderIndex", new string[] { "holderIndex.js" });
            addViewScripts("transfer", new string[] { "transfer.js" });
            addViewScripts("shareHolder", new string[] { "shareHolder.js" });
            addViewScripts("productDetails", new string[] { "productDetails.js" });
            addViewScripts("allUserStore", new string[] { "allUserStore.js" });
            addViewScripts("deliverCheck", new string[] { "deliverCheck.js" });
            addViewScripts("supplierProductCreate", new string[] { "supplierProductCreate.js" });
            addViewScripts("stockUserIndex", new string[] { "stockUserIndex.js" });
            addViewScripts("displayManageDetail", new string[] { "displayManageDetail.js" });
            addViewScripts("returns", new string[] { "returns.js" });
        }
    }
}
