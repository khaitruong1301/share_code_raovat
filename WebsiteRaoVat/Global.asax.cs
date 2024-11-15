using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace WebsiteRaoVat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //SqlDependency.Start(ConfigurationManager.ConnectionStrings["RaoVatDB"].ConnectionString);
            Application["Soluongtruycap"] = 0;
        }
        protected void Session_Start()
        {
            Application.Lock();
            Application["Soluongtruycap"] = (int)Application["Soluongtruycap"] + 1;
            Application.UnLock();

        }
        protected void Application_End()
        {
            //SqlDependency.Stop(ConfigurationManager.ConnectionStrings["RaoVatDB"].ConnectionString);
        }
    }
}
