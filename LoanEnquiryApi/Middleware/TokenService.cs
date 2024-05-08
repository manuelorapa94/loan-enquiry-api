using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoanEnquiryApi.Middleware
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken()
        {
            var randomId = Guid.NewGuid();
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, randomId.ToString()),
                new Claim(ClaimTypes.PrimarySid, randomId.ToString()),
                new Claim(ClaimTypes.Name, ""),
                new Claim(ClaimTypes.Email, ""),
                new Claim(ClaimTypes.GivenName, "")
            };

            var tokenKey = _config["JWT:SecretKey"];

            if (string.IsNullOrEmpty(tokenKey))
                return string.Empty;

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public JwtSecurityToken ReadToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

            return jwtSecurityToken;
        }
    }

}