using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using MVCClaseSeis.Models;
using System.Net.Http;
using Unity.Injection;

namespace MVCClaseSeis
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            // Crea el contenedor de Unity
            var container = new UnityContainer();

            // Configura las dependencias aquí
            container.RegisterType<AuthService>();
            container.RegisterType<ContactDAO>();
           container.RegisterType<PaymentDAO>();
            //container.RegisterType<HttpClient>(new InjectionFactory(_ => new HttpClient()));

            // Reemplazar InjectionFactory por RegisterFactory
            container.RegisterFactory<HttpClient>(c => new HttpClient());
            // Establece el contenedor de Unity como DependencyResolver de MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
