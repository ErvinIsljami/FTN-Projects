using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class CommentMappingProfile : Profile
    {
        public CommentMappingProfile()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(destination => destination.User,
                    opts => opts.MapFrom(source => source.User));
            //.ForMember(destination => destination.Ride,
            //    opts => opts.MapFrom(source => source.Ride));
            CreateMap<CommentDto, Comment>()
                .ForMember(destination => destination.User,
                    opts => opts.MapFrom(source => source.User));
            //.ForMember(destination => destination.Ride,
            //    opts => opts.MapFrom(source => source.Ride));
        }
    }
}