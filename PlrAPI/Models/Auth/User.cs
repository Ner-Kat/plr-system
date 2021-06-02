using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PlrAPI.Models.Auth
{
    public class User
    {
        // Первичный ключ, ID пользователя
        [Key]
        public int Id { get; set; }

        // Логин пользователя API
        [Required]
        public string Login { get; set; }

        // Пароль пользователя API (хэш пароля)
        [Required]
        public string Password { get; set; }

        // Соль хэша пароля
        [Required]
        public string Salt { get; set; }

        // Роль пользователя API
        [Required]
        public string Role { get; set; }

        // Обновляющий токен
        public string RefreshToken { get; set; }
    }
}
