using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMS.Core;

namespace UMS.BL.Helpers
{
    public static class CookieHelper
    {
        public static void SetCookie(HttpContext context, string key, string value, int? expireTime = null)
        {
            CookieOptions option = new CookieOptions();

            if (expireTime.HasValue)
                option.Expires = DateTime.UtcNow.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.UtcNow.AddDays(Constant.DEFAULT_COOKIE_EXPIRY);

            option.HttpOnly = true;
            option.Secure = true; // Set to true if your app uses HTTPS
            option.SameSite = SameSiteMode.None;

            context.Response.Cookies.Append(key, value, option);
        }

        public static string GetCookie(HttpContext context, string key)
        {
            return context.Request.Cookies[key];
        }

        public static void RemoveCookie(HttpContext context, string key)
        {
            context.Response.Cookies.Delete(key);
        }
    }
}
