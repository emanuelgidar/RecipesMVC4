using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace RecipesMVC4
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            new RavenDBAutofacModule().Resolve();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BootstrapSupport.BootstrapBundleConfig.RegisterBundles(System.Web.Optimization.BundleTable.Bundles);
            RecipesMVC4.ExampleLayoutsRouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}