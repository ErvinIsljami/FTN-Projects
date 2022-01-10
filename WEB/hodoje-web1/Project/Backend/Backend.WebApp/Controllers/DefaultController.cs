//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using Backend.DataAccess;
//using Backend.DataAccess.ModelRepositories;
//using Backend.DataAccess.UnitOfWork;
//using Backend.Models;
//using DomainEntities.Models;

//namespace Backend.Controllers
//{
//    public class DefaultController : ApiController
//    {
//        private readonly IUnitOfWork _unitOfWork;

//        public DefaultController(IUnitOfWork unitOfWork)
//        {
            //_unitOfWork = unitOfWork;
            //Location dl = new Location
            //{
            //    Address =
            //    {
            //        City = "NS",
            //        StreetName = "STR",
            //        StreetNumber = "123",
            //        PostalCode = "ABC123"
            //    },
            //    Longitude = 5,
            //    Latitude = 7
            //};
            //_unitOfWork.LocationRepository.Add(dl);
            //_unitOfWork.Complete();

            //User d = new User
            //{
            //    Username = "user",
            //    Password = "pass",
            //    Name = "nikola",
            //    Lastname = "karaklic",
            //    DriverLocationId = 1,
            //    Email = "email",
            //    Gender = 1,
            //    IsBanned = false,
            //    NationalIdentificationNumber = "1234",
            //    PhoneNumber = "12345",
            //    Role = 2,
            //    Car = new Car { CarType = 2, RegistrationNumber = "ABC123", YearOfManufactoring = 1996, TaxiNumber = "44" }
            //};
            //_unitOfWork.UserRepository.Add(d);

            //Location dll = new Location
            //{
            //    Address =
            //    {
            //        City = "NSS",
            //        StreetName = "STRR",
            //        StreetNumber = "12345",
            //        PostalCode = "ABC12345"
            //    },
            //    Longitude = 7,
            //    Latitude = 9
            //};
            //_unitOfWork.LocationRepository.Add(dll);
            //_unitOfWork.Complete();

            //User cust = new User
            //{
            //    Username = "custuser",
            //    Password = "custpass",
            //    Name = "custname",
            //    Lastname = "custlastname",
            //    Email = "email@email.com",
            //    Gender = (int)Gender.FEMALE,
            //    Role = (int)Role.CUSTOMER
            //};

            //_unitOfWork.UserRepository.Add(cust);
            //_unitOfWork.Complete();

            //User dd = new User
            //{
            //    Username = "username123",
            //    Password = "password123",
            //    Name = "dimitrije",
            //    Lastname = "nestorov",
            //    DriverLocationId = dl.Id,
            //    Email = "dimitrije@email.com",
            //    Gender = (int)Gender.MALE,
            //    IsBanned = false,
            //    Role = (int)Role.DRIVER,
            //};

            //_unitOfWork.UserRepository.Add(dd);
            //_unitOfWork.Complete();

            //Car c = new Car
            //{
            //    Id = dd.Id,
            //    CarType = (int)CarType.PASSENGER,
            //    DriverId = dd.Id,
            //    RegistrationNumber = "ABC123",
            //    TaxiNumber = 66,
            //    YearOfManufactoring = 2006
            //};

            //_unitOfWork.CarRepository.Add(c);
            //_unitOfWork.Complete();

            //dd.CarId = c.Id;
            //_unitOfWork.UserRepository.Update(dd);
            //_unitOfWork.Complete();

            //Ride r = new Ride
            //{
            //    CarType = 1,
            //    Comment = new Comment
            //    {
            //        Description = "poruka",
            //        Rating = 3,
            //        Timestamp = DateTime.Now,
            //        User = cust
            //    },
            //    CommentId = null,
            //    Customer = cust,
            //    Driver = dd,
            //    StartLocation = dll
            //};

            //_unitOfWork.RideRepository.Add(r);
            //_unitOfWork.Complete();

        //}

        //public User GetDefault()
        //{
        //    //User u = _unitOfWork.UserRepository.Find(x => x.IsBanned == false).First();
        //    //return u;
        //    return null;
        //}
    //}
//}
