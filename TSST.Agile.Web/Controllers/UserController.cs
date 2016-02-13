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
        public async Task<UserViewModel> GetProfile()
        {
            var user = await _userRepository.EntityFindByAsync(x => x.FacebookId == _fbId);
            if (user != null)
            {
                var userViewModel = Mapper.Map<UserViewModel>(user);
                return userViewModel;
            }
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }

        [HttpGet]
        public async Task<ICollection<UserViewModel>> GetFriends()
        {
            var user = await _userRepository.EntityFindByAsync(x => x.FacebookId == _fbId);
            if (user != null)
            {
                var userFriends = user.Friendships.Select(x => x.Friend);
                var userViewModelList = Mapper.Map<IEnumerable<User>, ICollection<UserViewModel>>(userFriends);
                return userViewModelList;
            }
            else
                throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }
}