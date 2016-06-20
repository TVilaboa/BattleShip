using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BattleShip.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


namespace BattleShip.MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
          
           
            AreaRegistration.RegisterAllAreas();
            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            });
          
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
