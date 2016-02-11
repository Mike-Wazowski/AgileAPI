using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TSST.Agile.Database.Configuration.Interfaces;
using TSST.Agile.Database.Models;
using TSST.Agile.Models;

namespace TSST.Agile.Web.Controllers
{
    [Authorize]
    public class UserController : IdentityApiController
    {
        private IGenericRepository<User> _userRepository;

        public UserController(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ApplicationUser> GetProfile()
        {
            var user = await _userRepository.EntityFindByAsync(x => x.FacebookId == _fbId);
            if (user != null)
            {
                var applicationUser = Mapper.Map<ApplicationUser>(user);
                return applicationUser;
            }
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<ICollection<ApplicationUser>> GetFriends()
        {
            var user = await _userRepository.EntityFindByAsync(x => x.FacebookId == _fbId);
            if (user != null)
            {
                var userFriends = user.Friendships.Select(x => x.Friend).ToList();
                var applicationUserList = Mapper.Map<ICollection<User>, ICollection<ApplicationUser>>(userFriends);
                return applicationUserList;
            }
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}