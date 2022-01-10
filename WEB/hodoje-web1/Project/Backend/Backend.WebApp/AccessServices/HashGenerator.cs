using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using Backend.Models;

namespace Backend.AccessServices
{
    public class HashGenerator
    {
        public string GenerateHash(LoginModel user)
        {
            string combineLoginData = $"{user.Username}{user.Password}";
            byte[] dataToBeHashed = Encoding.UTF8.GetBytes(combineLoginData);
            byte[] hash = {};
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = shaM.ComputeHash(dataToBeHashed);
            }
            string hashStringified = Encoding.UTF8.GetString(hash);
            return hashStringified;
        }
    }
}