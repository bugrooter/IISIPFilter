using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace IISIPFilter
{
    public class FilterIPHttpModule : IHttpModule
    {

        public FilterIPHttpModule()
        {

        }

        //Init方法用来注册HttpApplication 事件。
        public void Init(HttpApplication r_objApplication)
        {
            r_objApplication.BeginRequest += new EventHandler(this.BeginRequest);
        }

        public void Dispose()
        {

        }

        private void BeginRequest(object r_objSender, EventArgs r_objEventArgs)
        {
            HttpApplication objApp = (HttpApplication)r_objSender;
            HttpResponse Response = objApp.Response;
            HttpRequest Request = objApp.Request;


            if (System.Web.HttpRuntime.Cache["__IIS__IP__Filter__"] == null)
            {
                IpFilterSettings tmodel = System.Configuration.ConfigurationManager.GetSection("IpFilterSettings") as IpFilterSettings;
                System.Web.HttpRuntime.Cache.Insert("__IIS__IP__Filter__", tmodel, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromHours(12));
            }
            IpFilterSettings ipFilterSettings = (IpFilterSettings)System.Web.HttpRuntime.Cache["__IIS__IP__Filter__"];

            if (ipFilterSettings != null && ipFilterSettings.IpFilterBlackList != null && ipFilterSettings.IpFilterFile != null)
            {

                string filter_ip_black_list = ipFilterSettings.IpFilterBlackList.Ips;
                string files = ipFilterSettings.IpFilterFile.files;
                string ext = System.IO.Path.GetExtension(Request.Url.AbsoluteUri) + "";
                ext = ext.ToLower().Trim();
                if (files == "*" || (ext!="" && files.IndexOf(ext) > -1))
                {
                    string remoteip = Comm.GetRemoteIp();
                    string[] ips = filter_ip_black_list.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                    bool hasAllow = true;
                    long ipi = Comm.IpToInt(remoteip);
                    foreach (var item in ips)
                    {
                        long it = Comm.IpToInt(item);
                        if (it == ipi)
                        {
                            hasAllow = false;
                            break;
                        }
                    }
                    if (hasAllow==true)
                    {
                        List<IPModel> list = (List<IPModel>)ipFilterSettings.IpFilterList.List;
                        hasAllow = false;
                        foreach (var item in list)
                        {
                            if (ipi >= item.Min && ipi <= item.Max)
                            {
                                hasAllow = true;
                                break;
                            }
                        }
                    }
                    
                    if (!hasAllow)
                    {
                        Response.Write("<p style=\"text-align: center;color:red;font-size:20px;margin-top:50px;\">您的IP[" + remoteip + "]无权限访问</p>");
                        Response.End();
                    }
                }
            }
        }
    }
}
