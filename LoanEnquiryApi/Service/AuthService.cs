using LoanEnquiryApi.Entity;
using LoanEnquiryApi.Middleware;
using Microsoft.AspNetCore.Identity;

namespace LoanEnquiryApi.Service
{
    public class AuthService(UserManager<UserEntity> userManager, TokenService tokenService)
    {
        private readonly UserManager<UserEntity> _userManager = userManager;
        private readonly TokenService _tokenService = tokenService;

        public string? IsValidLogin(string username, string password)
        {
            var user = _userManager.FindByNameAsync(username).Result;

            if (user == null)
                return null;

            if (_userManager.CheckPasswordAsync(user, password).Result)
            {
                var token = _tokenService.CreateToken();

                return token;
            }

            return null;
        }
    }
}
