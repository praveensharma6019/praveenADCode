using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sitecore.Feature.Accounts.Helper
{
    public static class HTTPContextHelper
    {
        public static string ToHashString(this HttpBrowserCapabilities browser)
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(browser.Browser);
            myStr.Append(browser.Platform);
            myStr.Append(browser.MajorVersion);
            myStr.Append(browser.MinorVersion);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            return System.Convert.ToBase64String(hashdata);
        }

        public static string ToHashString(this HttpBrowserCapabilitiesBase browser)
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(browser.Browser);
            myStr.Append(browser.Platform);
            myStr.Append(browser.MajorVersion);
            myStr.Append(browser.MinorVersion);
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] hashdata = sha.ComputeHash(Encoding.UTF8.GetBytes(myStr.ToString()));
            return System.Convert.ToBase64String(hashdata);
        }

        public static void ClearSession(this HttpContext context)
        {
            context.Session.RemoveAll();
            context.Session.Clear();
            context.Session.Abandon();
            context.Response.Cookies["ASP.NET_SessionId"].Expires = System.DateTime.Now.AddSeconds(-30);
            context.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            context.Response.Cookies.Clear();
        }

        public static void ClearSession(this HttpSessionStateBase session)
        {
            session.RemoveAll();
            session.Clear();
            session.Abandon();
        }

        public static void DestroyCookie(this HttpResponseBase response)
        {
            response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(-30);
            response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            response.Cookies.Clear();
        }
    }
}