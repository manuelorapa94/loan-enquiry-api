using LoanEnquiryApi.Entity;
using LoanEnquiryApi.Middleware;
using LoanEnquiryApi.Model.Auth;
using LoanEnquiryApi.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoanEnquiryApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(UserManager<UserEntity> userManager, TokenService tokenService) : Controller
    {
        private readonly AuthService _service = new AuthService(userManager, tokenService);

        [HttpPost("login")]
        public IActionResult Login(LoginParam param)
        {
            var token = _service.IsValidLogin(param.Username, param.Password);

            if (string.IsNullOrEmpty(token)) return Unauthorized();

            return Ok(token);
        }

        //[HttpGet("validate")]
        //public IActionResult Validate()
        //{
        //    var authorization = Request.Headers.Authorization.ToString();
        //    if (AuthenticationHeaderValue.TryParse(authorization, out var parsedValue))
        //    {
        //        return Ok();
        //    }

        //    return Unauthorized();
        //}
    }
}
