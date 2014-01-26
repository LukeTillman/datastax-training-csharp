using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Castle.Windsor;
using Playlist.Controllers;

namespace Playlist
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _container;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Register all components with the Windsor IoC container and tell MVC to use Windsor to create controller instances
            _container = WindsorConfig.CreateContainer();
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(_container.Kernel));

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_End()
        {
            // Dispose of the container on application end
            if (_container != null)
                _container.Dispose();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            if (Context.Response.StatusCode != (int) HttpStatusCode.NotFound) return;

            // Handle 404s by executing another controller action rather than turning on customErrors so developers still see 
            // the "Yellow Screen of Death" for errors other that 404s, but we still get the same "Not Implemented" custom 404
            // page as the Java training code
            Response.Clear();

            var rd = new RouteData();
            rd.DataTokens["area"] = string.Empty;
            rd.Values["controller"] = "Errors";
            rd.Values["action"] = "NotFound";

            IController c = new ErrorsController();
            c.Execute(new RequestContext(new HttpContextWrapper(Context), rd));
        }
    }
}