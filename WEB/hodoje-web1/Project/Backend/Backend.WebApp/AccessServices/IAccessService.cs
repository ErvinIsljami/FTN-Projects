using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Models;
using Backend.DataAccess.UnitOfWork;

namespace Backend.AccessServices
{
    public interface IAccessService
    {
        ApiMessage<string, LoginModel> Login(ApiMessage<string, LoginModel> user, IUnitOfWork unitOfWork);
        bool Logout(string hash);
        bool IsLoggedIn(string hash);
        bool BlockUser(string username, IUnitOfWork unitOfWork);
        bool UnblockUser(string username, IUnitOfWork unitOfWork);
        bool IsAuthorized(string hash, string[] roles);
        ApiMessage<string, LoginModel> GetLoginData(string userHash, IUnitOfWork unitOfWork);
        string ExtractHash(string encodedHash);
    }
}
