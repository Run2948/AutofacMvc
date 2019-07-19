using Autofac;
using Autofac.Integration.Mvc;
using AutoFacMvc.Models;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System.Data.Entity;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutoFacMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // If we're using EntityFramework 6, here's where it'd go.
            // This is in the MiniProfiler.EF6 NuGet package.
            MiniProfilerEF6.Initialize();

            var builder = new ContainerBuilder();

            //register controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            //register repository
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

            //add the Entity Framework context to make sure only one context per request
            builder.RegisterType<SchoolContext>().InstancePerRequest();
            builder.Register(c => c.Resolve<SchoolContext>()).As<DbContext>().InstancePerRequest();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //ModelBinders.Binders.Add(typeof(Student), new StudentModelBinder());
        }

        protected void Application_BeginRequest()
        {
            // You can decide whether to profile here, or it can be done in ActionFilters, etc.
            // We're doing it here so profiling happens ASAP to account for as much time as possible.
            if (Request.IsLocal) // Example of conditional profiling, you could just call MiniProfiler.StartNew();
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop(); // Be sure to stop the profiler!
        }
    }
}
