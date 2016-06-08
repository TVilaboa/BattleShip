using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BattleShip.Services;

namespace BattleShip.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InitializationService.GetInstance.Initialize();
        }

        protected void Application_BeginRequest()
        {
            InitializationService.GetInstance.InstanceContext();
        }
        protected void Application_EndRequest()
        {
            InitializationService.GetInstance.DisposeContext();
        }
    }
}
