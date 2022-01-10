using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Backend.AccessServices;
using Backend.DataAccess.UnitOfWork;
using Backend.Models;
//using Backend.Models.CustomAttributes;
using DomainEntities.Models;

namespace Backend.AccessServices
{
    public class AccessService : IAccessService
    {
        private ICacheManager<string, LoginModel> _cacheManager;
        private HashGenerator _hashGenerator;
        private string key = "LoggedUsers";
        static object lockk = new object();

        public AccessService(ICacheManager<string, LoginModel> cacheManager, HashGenerator hashGenerator)
        {
            _cacheManager = cacheManager;
            _cacheManager.Set(key, new Dictionary<string, LoginModel>(), 24);
            _hashGenerator = hashGenerator;
        }

        public ApiMessage<string, LoginModel> Login(ApiMessage<string, LoginModel> user, IUnitOfWork unitOfWork)
        {
            ApiMessage<string, LoginModel> returnMessage = null;
            Dictionary<string, LoginModel> loggedUsers = (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");

            // This lock handles multiple fast same-parameter logins
            lock (lockk)
            {
                User userToLogin;
                if ((userToLogin = unitOfWork.UserRepository.Find(u => u.Username == user.Data.Username && u.Password == user.Data.Password).FirstOrDefault()) != null)
                {
                    if (!userToLogin.IsBanned)
                    {
                        string hash = _hashGenerator.GenerateHash(user.Data);

                        user.Data.Role = ((Role)unitOfWork.UserRepository.Find(u => u.Username == user.Data.Username).FirstOrDefault().Role).ToString();

                        if (!loggedUsers.ContainsKey(hash))
                        {
                            if (loggedUsers.Values.FirstOrDefault(u => u.Username == user.Data.Username) == null)
                            {
                                loggedUsers.Add(hash, user.Data);

                                returnMessage = new ApiMessage<string, LoginModel>();
                                returnMessage.Key = hash;
                                returnMessage.Data = user.Data;

                                _cacheManager.Set(key, loggedUsers, 24);
                            }
                        }
                    }
                }
            }
            return returnMessage;
        }

        public bool Logout(string hash)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");
            if (!String.IsNullOrWhiteSpace(hash))
            {
                if (loggedUsers.ContainsKey(hash))
                {
                    loggedUsers.Remove(hash);
                    result = true;
                }
                _cacheManager.Set(key, loggedUsers, 24);
            }
            return result;
        }

        public bool IsLoggedIn(string hash)
        {
            bool result = false;
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Key, u => u.Value);
            {
                if (loggedUsers.ContainsKey(hash))
                {
                    result = true;
                }
            }
            return result;
        }

        public ApiMessage<string, LoginModel> GetLoginData(string userHash, IUnitOfWork unitOfWork)
        {
            ApiMessage<string, LoginModel> loginData = null;
            Dictionary<string, LoginModel> loggedUsers =
                (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");
            if (loggedUsers.ContainsKey(userHash))
            {
                loginData = new ApiMessage<string, LoginModel>
                {
                    Key = userHash,
                    Data = loggedUsers[userHash]
                };
            }
            return loginData;
        }

        public string ExtractHash(string encodedHash)
        {
            byte[] data = System.Convert.FromBase64String(encodedHash);
            var escapedDecodedHash = System.Text.ASCIIEncoding.ASCII.GetString(data);
            var extractedHash = Uri.UnescapeDataString(escapedDecodedHash);
            return extractedHash;
        }

        public bool BlockUser(string username, IUnitOfWork unitOfWork)
        {
            User userToBlock = unitOfWork.UserRepository.Find(u => u.Username == username).FirstOrDefault();
            if (userToBlock != null)
            {
                if (!userToBlock.IsBanned)
                {
                    userToBlock.IsBanned = true;
                    unitOfWork.UserRepository.Update(userToBlock);
                    unitOfWork.Complete();
                    Dictionary<string, LoginModel> loggedUsers = (Dictionary<string, LoginModel>)_cacheManager.Get("LoggedUsers");
                    LoginModel loginModelForLoggedUser = loggedUsers.Values.FirstOrDefault(lm => lm.Username == username);
                    if (loginModelForLoggedUser != null)
                    {
                        string hash = loggedUsers.FirstOrDefault(pair => pair.Value == loginModelForLoggedUser).Key;
                        if (Logout(hash))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool UnblockUser(string username, IUnitOfWork unitOfWork)
        {
            User userToBlock = unitOfWork.UserRepository.Find(u => u.Username == username).FirstOrDefault();
            if (userToBlock != null)
            {
                if (userToBlock.IsBanned)
                {
                    userToBlock.IsBanned = false;
                    unitOfWork.UserRepository.Update(userToBlock);
                    unitOfWork.Complete();
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool IsAuthorized(string hash, string[] roles)
        {
            Dictionary<string, LoginModel> loggedUsers = _cacheManager.Get("LoggedUsers").ToDictionary(u => u.Key, u => u.Value);
            if (!String.IsNullOrWhiteSpace(hash))
            {
                if (roles.Contains(loggedUsers[hash].Role))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}