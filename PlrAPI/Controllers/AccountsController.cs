using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using PlrAPI.Models;
using PlrAPI.Models.Auth;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using PlrAPI.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace PlrAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private ApplicationContext _db;
        private AuthUtils _authUtils;
        private ILogger _logger;

        public AccountsController(ApplicationContext appContext, AuthUtils authUtils, ILogger<Startup> logger)
        {
            _db = appContext;
            _authUtils = authUtils;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Register(AuthData authData)
        {
            if (_db.Users.Any(u => u.Login == authData.Login))
            {
                return BadRequest("Login already exists");
            }

            byte[] salt = _authUtils.GenerateSalt();
            string hashedPassword = _authUtils.GetHashedPass(authData.Password, salt);

            try
            {
                User user = new User()
                {
                    Login = authData.Login,
                    Password = hashedPassword,
                    Salt = Convert.ToBase64String(salt),
                    Role = Roles.User
                };

                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch
            {
                return BadRequest();
            }

            return Ok();
        }

        [NonAction]
        private string GenerateTokens(User user)
        {
            // Создание токена
            ClaimsIdentity userIdentity = _authUtils.GetIdentity(user);
            string encodedJwt = _authUtils.CreateJwtToken(userIdentity);

            // Создание токена обновления
            _authUtils.UpdateRefreshToken(user);

            return encodedJwt;
        }

        [HttpPost]
        public IActionResult GetToken(AuthData authData)
        {
            _logger.LogInformation($"Запрос на получение токенов с данными {authData.Login}, {authData.Password}.");

            // Получение данных пользователя
            User user = _authUtils.AuthAndGetUser(authData.Login, authData.Password);
            if (user == null)
            {
                return BadRequest("Failed authenticate");
            }

            // Создание токенов
            string encodedJwt = GenerateTokens(user);

            // Отправка ответа
            var response = new
            {
                access_token = encodedJwt,
                refresh_token = user.RefreshToken
            };
            return new JsonResult(response);
        }

        [HttpPost]
        public IActionResult RefreshToken(TokenRefreshData tokenRefreshData)
        {
            // Получение данных пользователя
            User user = _authUtils.GetUserByRefreshToken(tokenRefreshData.RefreshToken);
            if (user == null)
            {
                return BadRequest("Wrong refresh token");
            }

            // Создание новых токенов
            string encodedJwt = GenerateTokens(user);

            // Отправка ответа
            var response = new
            {
                access_token = encodedJwt,
                refresh_token = user.RefreshToken
            };
            return new JsonResult(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword(AuthData authData)
        {
            int userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            User user = _authUtils.GetUserById(userId);

            byte[] salt = _authUtils.GenerateSalt();
            string hashedPassword = _authUtils.GetHashedPass(authData.Password, salt);

            user.Salt = Convert.ToBase64String(salt);
            user.Password = hashedPassword;
            _db.SaveChanges();

            _authUtils.UpdateRefreshToken(user);
            var response = new
            {
                refresh_token = user.RefreshToken
            };
            return Ok(response);
        }

        [NonAction]
        private JsonResult GetList(IQueryable<User> users, int? count, int? from)
        {
            if (count.HasValue)
            {
                return new JsonResult(users.TakeWhile((u, index) => index >= from && index < from + count).ToList());
            }

            return new JsonResult(users.ToList());
        }

        [Authorize(Policy = "ForAdmins")]
        [HttpGet]
        public JsonResult GetUsersList(int? count, int? from = 0)
        {
            return GetList(_db.Users, count, from);
        }

        [Authorize(Policy = "ForAdmins")]
        [HttpGet]
        public IActionResult ChangeUserRole(int userId, string role)
        {
            User user = _authUtils.GetUserById(userId);
            string currentRole = User.FindFirstValue(ClaimTypes.Role);

            if (role == Roles.SuperAdmin || (user.Role == Roles.Admin && currentRole != Roles.SuperAdmin) || user.Role == Roles.SuperAdmin)
            {
                return BadRequest("Role is not be able to change");
            }

            user.Role = role;
            _db.SaveChanges();

            return Ok();
        }
    }
}
