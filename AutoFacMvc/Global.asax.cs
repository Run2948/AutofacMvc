using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using AutoFacMvc.Models;
using StackExchange.Profiling;

namespace AutoFacMvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            MiniProfilerEF.Initialize();

            //Autofac初始化过程
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);//注册所有的Controller
            //开发环境下，使用Stub类
            //builder.RegisterAssemblyTypes(typeof (MvcApplication).Assembly).Where(
            //    t => t.Name.EndsWith("Repository") && t.Name.StartsWith("Stub")).AsImplementedInterfaces();
            //发布环境下，使用真实的数据访问层
            builder.RegisterAssemblyTypes(typeof(MvcApplication).Assembly).AsImplementedInterfaces().PropertiesAutowired();

            //注册builder, 实现one context per lifetime
            builder.RegisterType<SchoolContext>().As<DbContext>().InstancePerLifetimeScope();
            builder.RegisterType<SchoolContext>().InstancePerLifetimeScope();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(Student), new StudentModelBinder());
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            } 
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }
    }
}
