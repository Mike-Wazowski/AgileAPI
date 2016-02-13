using Autofac;
using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using TSST.Agile.Database.Configuration.Implementations;
using TSST.Agile.Database.Configuration.Interfaces;

namespace TSST.Agile.Web.App_Start
{
    public class AutofacConfig
    {
        public static HttpConfiguration ConfigureContainer(IAppBuilder app)
        {
            var configuration = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterWebApiFilterProvider(configuration);
            
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(x => x.Namespace.EndsWith(".Implementations"))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>)).InstancePerRequest();

            builder.RegisterType<AgileDbContext>().As<IAgileDbContext>().InstancePerRequest();
            LoadAndRegisterAssemblies(builder);

            var container = builder.Build();
            var resolver = new AutofacWebApiDependencyResolver(container);
            configuration.DependencyResolver = resolver;
            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(configuration);
            return configuration;
        }

        private static void LoadAndRegisterAssemblies(ContainerBuilder builder)
        {
            LoadAssemblies();
            RegisterAssemblies(builder);
        }

        private static void LoadAssemblies()
        {
            var assembliesNames = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            foreach (var assemblyName in assembliesNames)
            {
                Assembly.Load(assemblyName);
            }
        }

        private static void RegisterAssemblies(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    builder.RegisterAssemblyTypes(assembly)
                        .Where(x =>
                        {
                            if (x != null && x.Namespace != null)
                                return x.Namespace.EndsWith(".Implementations");
                            return false;
                        })
                        .AsImplementedInterfaces();
                }
            }
        }
    }
}