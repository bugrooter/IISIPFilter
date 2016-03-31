using System;
using System.Collections.Generic;
using System.Text;

namespace IISIPFilter
{
    public class Comm
    {
        public static string GetRemoteIp()
        {
            string remoteip = "";
            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                remoteip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(',')[0];
            }
            if (string.IsNullOrEmpty(remoteip))
            {
                remoteip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(remoteip))
            {
                remoteip = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            return remoteip;
        }

        public static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24 | long.Parse(items[1]) << 16 | long.Parse(items[2]) << 8 | long.Parse(items[3]);
        }
    }
}
