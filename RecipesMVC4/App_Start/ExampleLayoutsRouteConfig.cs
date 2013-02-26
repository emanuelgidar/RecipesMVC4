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

            routes.MapNavigationRoute<AccountController>("Register", c => c.Register(null));

            routes.MapNavigationRoute<AccountController>("Login", c => c.SignIn(null));

                 //   .AddChildRoute<AccountController>("Register", c => c.Register(null))
                  //  .AddChildRoute<AccountController>("Login", c => c.SignIn(null));
                 //   .AddChildRoute<AccountController>("SignIn", c => c.SignIn(null));

          //  routes.MapNavigationRoute<ExampleLayoutsController>("Example Layouts", c => c.Starter())
          //        .AddChildRoute<ExampleLayoutsController>("Marketing", c => c.Marketing())
          //        .AddChildRoute<ExampleLayoutsController>("Fluid", c => c.Fluid())
            //      .AddChildRoute<ExampleLayoutsController>("Sign In", c => c.SignIn())
                //;
        }
    }
}
