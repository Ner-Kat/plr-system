using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PlrAPI.Systems
{
    public class AuthOptions
    {
        // Издатель токена
        public const string Issuer = "PlrAPIAuthServer";

        // Пользователь токена
        public const string Audience = "PlrAPIClient";

        // Ключ для шифрования
        const string Key = "iakPcq'g?xE%q9!Cd#@w$!";

        // Время жизни токена (в минутах)
        public const int Lifetime = 10;  // 2

        // Размер соли (байт)
        public const int SaltSize = 32;

        // Размер Refresh Token (байт)
        public const int RefreshTokenSize = 32;

        // Время жизни Refresh Token (в минутах), сейчас - 30 дней
        public const int RefreshTokenLifetime = 43200;


        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
