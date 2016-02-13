using Facebook;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using TSST.Agile.Database.Configuration.Interfaces;
using TSST.Agile.Database.Models;

namespace TSST.Agile.Web.Security
{
    public class AuthTokenProvider: OAuthAuthorizationServerProvider
    {
        public const string _fbIdKey = "fbId";
        public const string _idKey = "appId";

        private IGenericRepository<User> _userRepository;

        public override async System.Threading.Tasks.Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;
            _userRepository = (IGenericRepository<User>)dependencyResolver.GetService(typeof(IGenericRepository<User>));
            context.Validated();
        }

        public override async System.Threading.Tasks.Task GrantCustomExtension(OAuthGrantCustomExtensionContext context)
        {
            if(context.GrantType.ToLower() == "facebook")
            {
                var fbClient = new FacebookClient(context.Parameters.Get("accesstoken"));
                dynamic mainDataResponse = await fbClient.GetTaskAsync("me", new { fields = "first_name, last_name, picture" });
                dynamic friendListResponse = await fbClient.GetTaskAsync("me/friends");
                var friendsResult = (IDictionary<string, object>)friendListResponse;
                var friendsData = (IEnumerable<object>)friendsResult["data"];
                var friendsIdList = new List<string>();
                foreach (var item in friendsData)
                {
                    var friend = (IDictionary<string, object>)item;
                    friendsIdList.Add((string)friend["id"]);
                }
                User user = await CreateOrUpdateUser(mainDataResponse.id, mainDataResponse.first_name, mainDataResponse.last_name, mainDataResponse.picture.data.url, friendsIdList);
                
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(_fbIdKey, mainDataResponse.id));
                identity.AddClaim(new Claim(_idKey, user.Id.ToString()));

                await base.GrantCustomExtension(context);
                context.Validated(identity);
            }
            return;
        }

        private async System.Threading.Tasks.Task<User> CreateOrUpdateUser(string fbId, string firstName, string lastName, string pictureUrl, List<string> friendsIdList)
        {
            var user = await _userRepository.EntityFindByAsync(x => x.FacebookId == fbId);
            if(user == null)
            {
                await CreateUser(fbId, firstName, lastName, pictureUrl, friendsIdList);
            }
            else
                await UpadateUser(user, friendsIdList);
            _userRepository.Save();
            return user;
        }

        private async System.Threading.Tasks.Task UpadateUser(User user, List<string> friendsIdList)
        {
            _userRepository.ExecCommand("DELETE FROM Friendships WHERE UserId = " + user.Id);
            var friends = await _userRepository.FindByAsync(x => friendsIdList.Contains(x.FacebookId));
            foreach (var friend in friends)
            {
                user.Friendships.Add(new Friendship() { User = user, Friend = friend });
            }
        }

        private async System.Threading.Tasks.Task CreateUser(string fbId, string firstName, string lastName, string pictureUrl, List<string> friendsIdList)
        {
            var user = new User() { FacebookId = fbId, FirstName = firstName, LastName = lastName, PictureUrl = pictureUrl };
            var friends = await _userRepository.FindByAsync(x => friendsIdList.Contains(x.FacebookId));
            foreach (var friend in friends)
            {
                user.Friendships.Add(new Friendship() { User = user, Friend = friend });
                friend.Friendships.Add(new Friendship() { User = friend, Friend = user });
            }
            _userRepository.Add(user);
        }
    }
}