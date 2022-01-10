using System.Data.Entity;
using System.Web.Http;
using AutoMapper;
using Backend.Controllers;
using Backend.DataAccess;
using Backend.DataAccess.ModelRepositories;
using Backend.DataAccess.ModelRepositoryInterfaces;
using Backend.DataAccess.UnitOfWork;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;
using Backend.App_Start.MappingProfiles;
using Backend.Models;
using Unity.Injection;
using Backend.AccessServices;

namespace Backend
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<DbContext, DatabaseContext>(new HierarchicalLifetimeManager());

            container.RegisterType<ICarRepository, CarRepository>();
            container.RegisterType<ICommentRepository, CommentRepository>();
            container.RegisterType<ILocationRepository, LocationRepository>();
            container.RegisterType<IRideRepository, RideRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            container.RegisterType<ICacheManager<string, LoginModel>, CacheManager<string, LoginModel>>();
            container.RegisterType<IAccessService, AccessService>(new ContainerControlledLifetimeManager());

            container.RegisterType<HashGenerator>();

            MapperConfiguration config = new MapperConfiguration(c =>
            {
                c.AddProfile<LoginModelMappingProfile>();
                c.AddProfile<CarMappingProfile>();
                c.AddProfile<CommentMappingProfile>();
                c.AddProfile<LocationMappingProfile>();
                c.AddProfile<RideMappingProfile>();
                c.AddProfile<UserMappingProfile>();
            });

            container.RegisterType<IMapper, Mapper>(new InjectionConstructor(config));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}