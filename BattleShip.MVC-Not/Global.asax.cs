using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BattleShip.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //WebSecurity.InitializeDatabaseConnection("MembershipDb", "Users", "Id", "Email", false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        //protected void Application_BeginRequest()
        //{
        //    InitializationService.GetInstance.InstanceContext();
        //}
        //protected void Application_EndRequest()
        //{
        //    InitializationService.GetInstance.DisposeContext();
        //}
    }
}
