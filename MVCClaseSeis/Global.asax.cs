using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVCClaseSeis
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Asegúrate de que esta línea esté presente para inicializar Unity
            UnityConfig.RegisterComponents();
        }
    }
}
