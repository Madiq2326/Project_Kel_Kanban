using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Kanban.Utililty
{
    public class Hashing
    {
        private static string getRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(10);
        }

        public static string hashPassword(string Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password, getRandomSalt());
        }

        public static bool validatePassword(string Password, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(Password, hashPassword);
        }
    }
}