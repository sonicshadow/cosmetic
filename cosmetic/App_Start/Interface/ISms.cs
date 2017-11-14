using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Cosmetic
{
    public interface ISms
    {
        SmsResult Send(string phone, string text);
    }
    public class CnSms : ISms
    {

        public SmsResult Send(string phone, string text)
        {
            SmsResult result = new SmsResult() { IsSuccess = false, Message = "" };
            try
            {
                var api = new Api.BaseApi($"http://utf8.sms.webchinese.cn/?Action=SMS_Num&Uid=cynet&Key=71f885c532d563441443&smsMob={phone}&smsText={text}", "GET");
                string r = api.CreateRequestReturnString();
                int code;
                int.TryParse(r, out code);
                switch (code)
                {
                    case -1:
                    case -2:
                    case -21:
                    case -41:
                    case -42:
                    case -51:
                        result.Message = "接口参数有误";
                        break;
                    case -3:
                        result.Message = "短信数量不足";
                        break;
                    case -11:
                        result.Message = "该用户被禁用";
                        break;
                    case -14:
                        result.Message = "短信内容出现非法字符";
                        break;
                    case -4:
                        result.Message = "手机号格式不正确";
                        break;
                    case -6:
                        result.Message = "手机号格式不正确";
                        break;
                    default:
                        result.IsSuccess = true;
                        result.Message = $"成功发送{code}条短信";
                        break;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"请求有误，{ex.Message}";
            }
            return result;
        }
        
    }

    public class SmsResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}