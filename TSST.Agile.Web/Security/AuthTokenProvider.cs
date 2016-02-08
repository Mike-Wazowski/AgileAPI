using Facebook;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace TSST.Agile.Web.Security
{
    public class AuthTokenProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            if(context.GrantType.ToLower() == "facebook")
            {
                var fbClient = new FacebookClient(context.Parameters.Get("accesstoken"));
                dynamic mainDataResponse = await fbClient.GetTaskAsync("me", new { fields = "first_name, last_name, picture" });
                dynamic friendListResponse = await fbClient.GetTaskAsync("me/friends");
                string id = mainDataResponse.id;
                string firstName = mainDataResponse.first_name;
                string lastName = mainDataResponse.last_name;
                string pictureUrl = mainDataResponse.picture.data.url;
                //create user here
                
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Name, firstName));
                identity.AddClaim(new Claim(ClaimTypes.Surname, lastName));
                identity.AddClaim(new Claim("id", id));

                await base.GrantCustomExtension(context);
                context.Validated(identity);
            }
            return;
        }
    }
}