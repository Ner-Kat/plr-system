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
        public IActionResult Register(string login, string password)
        {
            if (_db.Users.Any(u => u.Login == login))
            {
                return BadRequest("Login already exists");
            }

            byte[] salt = _authUtils.GenerateSalt();
            string hashedPassword = _authUtils.GetHashedPass(password, salt);

            try
            {
                User user = new User()
                {
                    Login = login,
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
        public IActionResult GetToken(string login, string password)
        {
            // Получение данных пользователя
            User user = _authUtils.AuthAndGetUser(login, password);
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
        public IActionResult RefreshToken(string refreshToken)
        {
            // Получение данных пользователя
            User user = _authUtils.GetUserByRefreshToken(refreshToken);
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
        public IActionResult ChangePassword(string password)
        {
            int userId = Convert.ToInt32(User.FindFirstValue("UserId"));
            User user = _authUtils.GetUserById(userId);

            byte[] salt = _authUtils.GenerateSalt();
            string hashedPassword = _authUtils.GetHashedPass(password, salt);

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

        [Authorize]
        [HttpGet]
        public JsonResult GetUsersList(int? count, int? from = 0)
        {
            return GetList(_db.Users, count, from);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangeUserRole(int userId, string role)
        {
            User user = _authUtils.GetUserById(userId);
            user.Role = role;
            _db.SaveChanges();

            return Ok();
        }
    }
}
