using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlrAPI.Systems
{
    public class Roles
    {
        public const string User = "user";
        public const string Admin = "admin";
        public const string SuperAdmin = "superadmin";

        public static string[] AllRoles()
        {
            string[] roles = new string[]
            {
                User, Admin, SuperAdmin
            };

            return roles;
        }
    }
}
