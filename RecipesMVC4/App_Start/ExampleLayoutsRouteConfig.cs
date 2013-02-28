using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RecipesMVC4.Controllers;
using NavigationRoutes;

namespace RecipesMVC4
{
    public class ExampleLayoutsRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapNavigationRoute<HomeController>("Home", c => c.Index());

            
            routes.MapNavigationRoute<AdminController>("Admin", c => c.Details(Guid.NewGuid()))
                .AddChildRoute<AdminController>("List Recipes", c => c.Index())
               .AddChildRoute<AdminController>("Create Recipe", c => c.Create(null));
        }
    }
}
