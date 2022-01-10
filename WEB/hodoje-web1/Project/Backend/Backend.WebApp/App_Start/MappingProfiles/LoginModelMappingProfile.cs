using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using Backend.Models;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class LoginModelMappingProfile : Profile
    {
        public LoginModelMappingProfile()
        {
            CreateMap<UserDto, LoginModel>(MemberList.Destination);
            CreateMap<User, LoginModel>(MemberList.Destination);
        }
    }
}