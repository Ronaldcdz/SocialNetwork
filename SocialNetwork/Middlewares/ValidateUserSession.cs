using Microsoft.AspNetCore.Http;
using SocialNetwork.Core.Application.Dtos.Accounnts;
using SocialNetwork.Core.Application.Helpers;

namespace SocialNetwork.Middlewares
{
    public class ValidateUserSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateUserSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool HasUser()
        {
            AuthenticationResponse userVm = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            
            if(userVm == null)
            {
                return false;
            }
            return true;

        }
    }
}
