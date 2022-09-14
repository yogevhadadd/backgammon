using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TalkBackAccessControll.Date.DB;
using TalkBackAccessControll.Date.Models;

namespace TalkBackAccessControll.Date.Services
{
    public class LoginService : ILoginService
    {
        private readonly TalkBackDbContext _context;
        private readonly IUserRepository _repositoriy;
        private SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
        private SigningCredentials signinCredentials;
        public LoginService(IUserRepository repositoriy, TalkBackDbContext context)
        {
            signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            _repositoriy = repositoriy;
            _context = context;
        }
        public Token Login(LoginRequest loginRequest)
        {
            User user = _repositoriy.Get(loginRequest.UserName);
            if (user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Role, "Manager"), //for managers
                new Claim(ClaimTypes.NameIdentifier, loginRequest.UserName),
                new Claim(ClaimTypes.Expired, DateTime.Now.AddMinutes(5).ToString()),
            };
            var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7048",
                    audience: "https://localhost:7048",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(100),
                    signingCredentials: signinCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            _context.SaveChanges();
            return new Token { AccessToken = tokenString, NickName = user.DisplayName, RefreshToken = refreshToken/*, RefreshTokenExpiryTime = DateTime.Now.AddDays(7)*/ };
        }
       
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345")),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
        public Token Refresh(Token tokenApiModel)
        {
            if (tokenApiModel is null)
                return null;
            string accessToken = tokenApiModel.AccessToken;
            string refreshToken = tokenApiModel.RefreshToken;
            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var user = _context.Users.Single(user => tokenApiModel.NickName == user.DisplayName);
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return null;
            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            return new Token()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7048",
                audience: "https://localhost:7048",
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
