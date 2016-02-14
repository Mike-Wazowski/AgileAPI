using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TSST.Agile.FileStorage.Interfaces;

namespace TSST.Agile.Web.Controllers
{
    public class TestController : ApiController
    {
        private IFileStorage _fileStorage;

        public TestController(IFileStorage fileStorage)
        {
            _fileStorage = fileStorage;
        }

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

        [HttpGet]
        public async Task<string> AddFile()
        {
            var ms = new MemoryStream();
            var stringBytes = System.Text.Encoding.UTF8.GetBytes("TEST 1");
            ms.Write(stringBytes, 0, stringBytes.Length);
            var url = await _fileStorage.AddFile(1, 33, "plik2.txt", ms);
            return url;
        }
    }
}