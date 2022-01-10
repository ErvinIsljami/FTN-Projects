using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Backend.Dtos;
using DomainEntities.Models;

namespace Backend.App_Start.MappingProfiles
{
    public class RideMappingProfile : Profile
    {
        public RideMappingProfile()
        {
            CreateMap<Ride, RideDto>()
                .ForMember(destination => destination.RideStatus,
                    opts => opts.MapFrom(source => ((RideStatus) source.RideStatus).ToString()))
                .ForMember(destination => destination.CarType,
                    opts => opts.MapFrom(source => ((CarType) source.CarType).ToString()))
                .ForMember(destination => destination.StartLocation,
                    opts => opts.MapFrom(source => source.StartLocation))
                .ForMember(destination => destination.DestinationLocation,
                    opts => opts.MapFrom(source => source.DestinationLocation))
                .ForMember(destination => destination.Customer,
                    opts => opts.MapFrom(source => source.Customer))
                .ForMember(destination => destination.Dispatcher,
                    opts => opts.MapFrom(source => source.Dispatcher))
                .ForMember(destination => destination.Driver,
                    opts => opts.MapFrom(source => source.Driver))
                .ForMember(destination => destination.Comments,
                    opts => opts.MapFrom(source => source.Comments));
            CreateMap<RideDto, Ride>()
                .ForMember(destination => destination.RideStatus,
                    opts => opts.MapFrom(source => Enum.GetValues(typeof(RideStatus)).Cast<RideStatus>().FirstOrDefault(rs => rs.ToString() == source.RideStatus)))
                .ForMember(destination => destination.CarType,
                    opts => opts.MapFrom(source => Enum.GetValues(typeof(CarType)).Cast<CarType>().FirstOrDefault(ct => ct.ToString() == source.CarType)))
                .ForMember(destination => destination.StartLocation,
                    opts => opts.MapFrom(source => source.StartLocation))
                .ForMember(destination => destination.DestinationLocation,
                    opts => opts.MapFrom(source => source.DestinationLocation))
                .ForMember(destination => destination.Customer,
                    opts => opts.MapFrom(source => source.Customer))
                .ForMember(destination => destination.Dispatcher,
                    opts => opts.MapFrom(source => source.Dispatcher))
                .ForMember(destination => destination.Driver,
                    opts => opts.MapFrom(source => source.Driver))
                .ForMember(destination => destination.Comments,
                    opts => opts.MapFrom(source => source.Comments));
        }
    }
}