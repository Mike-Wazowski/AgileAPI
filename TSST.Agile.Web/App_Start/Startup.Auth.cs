using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSST.Agile.Web.Security;

namespace TSST.Agile.Web
{
    public partial class Startup
    {
        private void ConfigureAuth(IAppBuilder app)
        {
            var OAuthserverOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                Provider = new AuthTokenProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthserverOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}