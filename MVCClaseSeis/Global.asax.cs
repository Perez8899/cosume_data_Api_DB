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

            // Aseg�rate de que esta l�nea est� presente para inicializar Unity
            UnityConfig.RegisterComponents();

        }
    }
}
