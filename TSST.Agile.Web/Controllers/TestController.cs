using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace TSST.Agile.Web.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        [HttpGet]
        public ICollection<string> GetNames()
        {
            var identity = (ClaimsIdentity)User.Identity;
            List<Claim> claims = identity.Claims.ToList();
            return new List<string>()
            {
                "Jurek",
                "Marta",
                "Krzysiek",
                "Anna"
            };
        }
    }
}