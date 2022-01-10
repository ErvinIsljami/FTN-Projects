using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Backend.DataAccess.UnitOfWork;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.Controllers
{
    public class RegistrationController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _iMapper;

        public RegistrationController(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
        }

        [HttpPost]
        public IHttpActionResult Register(UserDto userDto)
        {
            User user = _iMapper.Map<UserDto, User>(userDto);

            if (_unitOfWork.UserRepository.Find(u => u.Username == user.Username).Any())
            {
                return Conflict();
            }

            if (user.Role == (int)Role.DISPATCHER || user.Role == (int)Role.DRIVER)
            {
                return ResponseMessage(new HttpResponseMessage(HttpStatusCode.Forbidden));
            }

            user.Role = (int)Role.CUSTOMER;         // Role is not passed through registration form
            user.IsBanned = false;                  // Initially customer is not banned
            user.NationalIdentificationNumber = (String.IsNullOrWhiteSpace(user.NationalIdentificationNumber)) ? null : user.NationalIdentificationNumber;
            user.PhoneNumber = (String.IsNullOrWhiteSpace(user.PhoneNumber)) ? null : user.PhoneNumber;

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.Complete();

            userDto.Id = user.Id;
            return CreatedAtRoute("DefaultApi", new { controller = "Users", id = userDto.Id}, userDto);
        }
    }
}
