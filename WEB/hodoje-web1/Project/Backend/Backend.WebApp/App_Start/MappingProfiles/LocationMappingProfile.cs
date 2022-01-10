using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<Location, LocationDto>()
                .ForMember(destination => destination.Address,
                opts => opts.MapFrom(source => source.Address));
            CreateMap<LocationDto, Location>();
        }
    }
}