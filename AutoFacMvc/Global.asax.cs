using System;
using Autofac;
using Autofac.Integration.Mvc;
using AutoFacMvc.Models;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AutoFacMvc.Attributes;
using AutoFacMvc.Common.Logging;
using AutoFacMvc.Common.Models;

namespace AutoFacMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // If we're using EntityFramework 6, here's where it'd go.
            // This is in the MiniProfiler.EF6 NuGet package.
            MiniProfilerEF6.Initialize();

            // register binders
            ModelBinders.Binders.Add(typeof(SessionInfo), new SessionModelBinder());

            // register auto mapper
            AutoMapperConfig.Register();

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

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            //记录日志信息  
            LogManager.Error(exception);
#if !DEBUG
            //如果为空则走自定义
            var httpStatusCode = (exception as HttpException)?.GetHttpCode() ?? 700; 
            var httpContext = ((MvcApplication)sender).Context;
            httpContext.ClearError();
            //直接跳转到对应错误页面
            switch (httpStatusCode)
            {
                case 404:
                    httpContext.Response.Redirect("/Error/404.html");
                    break;
                default:
                    httpContext.Response.Redirect("/Error/500.html");
                    break;
            }
#endif
        }

    }
}
