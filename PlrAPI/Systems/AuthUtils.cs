using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PlrAPI.Models;
using PlrAPI.Models.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PlrAPI.Systems
{
    public class AuthUtils
    {
        private ApplicationContext _db;
        private HttpContext _httpContext;
        private ILogger _logger;

        public AuthUtils(ApplicationContext appContext, IHttpContextAccessor httpContextAccessor, ILogger<Startup> logger)
        {
            _db = appContext;
            _httpContext = httpContextAccessor.HttpContext;
            _logger = logger;
        }

        public string GetHashedPass(string password, byte[] salt)
        {
            return PasswordsUtils.CreateHashedPass(password, salt);
        }

        public byte[] GenerateSalt()
        {
            return PasswordsUtils.CreateSalt();
        }

        public ClaimsIdentity GetIdentity(User user)
        {
            ClaimsIdentity claimsIdentity = null;
            if (user != null)
            {
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
                    new Claim("UserId", user.Id.ToString())
                };

                claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }

            return claimsIdentity;
        }

        public User AuthAndGetUser(string login, string password)
        {
            User user = _db.Users.FirstOrDefault(u => u.Login == login);
            if (user != null)
            {
                string hashedPassword = GetHashedPass(password, Convert.FromBase64String(user.Salt));

                if (user.Password != hashedPassword)
                {
                    return null;
                }
            }

            return user;
        }

        public string CreateRefreshToken()
        {
            byte[] refreshToken = new byte[AuthOptions.RefreshTokenSize];
            var rndGenerator = RandomNumberGenerator.Create();
            rndGenerator.GetBytes(refreshToken);

            return Convert.ToBase64String(refreshToken);
        }

        public void UpdateRefreshToken(User user)
        {
            string refreshToken = CreateRefreshToken();

            // Обновление записи в БД
            user.RefreshToken = refreshToken;
            _db.SaveChanges();
        }

        public string CreateJwtToken(ClaimsIdentity userIdentity)
        {
            var now = DateTime.UtcNow;

            // Формирование токена
            JwtSecurityToken userToken = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: now,
                claims: userIdentity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(userToken);

            return encodedJwt;
        }

        public User GetUserByRefreshToken(string refreshToken)
        {
            return _db.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
        }

        public User GetUserById(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

    }
}
