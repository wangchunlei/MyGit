using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace HttpClient
{
    public class CookieManger
    {
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookie(string url, string cookieName, StringBuilder cookieData, ref int size);
        public static CookieContainer GetUriCookieContainer(Uri uri)
        {
            CookieContainer cookies = null;
            //定义Cookie数据的大小。
            int datasize = 256;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (!InternetGetCookie(uri.ToString(), ".ASPXAUTH", cookieData, ref datasize))
            {
                if (datasize < 0) return null;
                // 确信有足够大的空间来容纳Cookie数据。
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookie(uri.ToString(), ".ASPXAUTH", cookieData, ref datasize)) return null;
            }
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }
    }
}
