using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TSST.Agile.Web;
using TSST.Agile.Web.App_Start;

[assembly: OwinStartup(typeof(Startup))]
namespace TSST.Agile.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = AutofacConfig.ConfigureContainer(app);
            WebApiConfig.Register(config);
            ConfigureAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}