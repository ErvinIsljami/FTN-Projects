using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Backend.DataAccess;
using Backend.DataAccess.UnitOfWork;
using Backend.Dtos;
using Backend.Models.CustomAttributes;
using DomainEntities.Models;

namespace Backend.Controllers
{
    [AuthenticationFilter]
    public class CarsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _iMapper;

        public CarsController(IUnitOfWork unitOfWork, IMapper iMapper)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
        }

        //// GET: api/Cars
        //[HttpGet]
        //[ResponseType(typeof(IEnumerable<CarDto>))]
        //public IHttpActionResult GetCars()
        //{
        //    IEnumerable<Car> cars = _unitOfWork.CarRepository.GetAll();
        //    if (cars == null || cars.Count() < 1)
        //    {
        //        return NotFound();
        //    }
        //    IEnumerable<CarDto> carDtos = _iMapper.Map<IEnumerable<Car>, IEnumerable<CarDto>>(cars);
        //    return Ok(carDtos);
        //}

        //// GET: api/Cars/5
        //[HttpGet]
        //[ResponseType(typeof(CarDto))]
        //public IHttpActionResult GetCar(int id)
        //{
        //    Car car = _unitOfWork.CarRepository.GetById(id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }
        //    CarDto carDto = _iMapper.Map<Car, CarDto>(car);
        //    return Ok(carDto);
        //}

        // PUT: api/Cars/5
        [HttpPut]
        [ResponseType(typeof(void))]
        [AuthorizationFilter(new string[] { "DRIVER" })]
        [Route("api/cars/updateCar/{id}")]
        public IHttpActionResult PutCar(int id, CarDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carDto.Id)
            {
                return BadRequest();
            }

            Car car = _iMapper.Map<CarDto, Car>(carDto);

            try
            {
                _unitOfWork.CarRepository.Update(car);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        //// POST: api/Cars
        //[HttpPost]
        //[ResponseType(typeof(CarDto))]
        //public IHttpActionResult PostCar(CarDto carDto)             // Since Car's Id is the same as the DriverId, no need for some other action
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Car car = _iMapper.Map<CarDto, Car>(carDto);

        //    try
        //    {
        //        _unitOfWork.CarRepository.Add(car);
        //        _unitOfWork.Complete();

        //        User user = _unitOfWork.UserRepository.GetById(car.Id);
        //        user.CarId = car.Id;
        //        _unitOfWork.UserRepository.Update(user);
        //        _unitOfWork.Complete();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CarExists(car.Id))
        //        {
        //            return Conflict();
        //        }
        //    }

        //    _iMapper.Map<Car, CarDto>(car, carDto);

        //    return CreatedAtRoute("DefaultApi", new { id = carDto.Id }, carDto);
        //}

        //// DELETE: api/Cars/5
        //[HttpDelete]
        //[ResponseType(typeof(CarDto))]
        //public IHttpActionResult DeleteCar(int id)
        //{
        //    Car car = _unitOfWork.CarRepository.GetById(id);
        //    CarDto carDto = _iMapper.Map<Car, CarDto>(car);
        //    User user = _unitOfWork.UserRepository.GetById(id);     // Car's Id is same as the driver's
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    _unitOfWork.CarRepository.Remove(car);
        //    _unitOfWork.Complete();

        //    user.CarId = null;
        //    _unitOfWork.UserRepository.Update(user);
        //    _unitOfWork.Complete();

        //    return Ok(carDto);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CarExists(int id)
        {
            return _unitOfWork.CarRepository.GetById(id) != null;
        }
    }
}