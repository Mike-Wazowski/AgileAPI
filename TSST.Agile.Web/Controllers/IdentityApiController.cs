using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using TSST.Agile.Web.Security;

namespace TSST.Agile.Web.Controllers
{
    public abstract class IdentityApiController: ApiController
    {
        protected readonly string _fbId = string.Empty;
        protected readonly int _id = -1;

        public IdentityApiController()
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<Claim> claims = identity.Claims.ToList();
            var fbIdClaim = claims.Where(x => x.Type == AuthTokenProvider._fbIdKey).FirstOrDefault();
            var idClaim = claims.Where(x => x.Type == AuthTokenProvider._idKey).FirstOrDefault();
            if (fbIdClaim != null)
                _fbId = fbIdClaim.Value;
            if (idClaim != null)
            {
                Int32.TryParse(idClaim.Value, out _id);
            }
        }
    }
}