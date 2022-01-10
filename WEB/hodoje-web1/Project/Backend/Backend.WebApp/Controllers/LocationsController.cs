using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Backend.AccessServices;
using Backend.DataAccess;
using Backend.DataAccess.UnitOfWork;
using Backend.Dtos;
using Backend.Models;
using Backend.Models.CustomAttributes;
using DomainEntities.Models;

namespace Backend.Controllers
{
    [AuthenticationFilter]
    public class LocationsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _iMapper;
        private IAccessService _accessService;

        public LocationsController(IUnitOfWork unitOfWork, IMapper iMapper, IAccessService accessService)
        {
            _unitOfWork = unitOfWork;
            _iMapper = iMapper;
            _accessService = accessService;
        }

        // GET: api/Locations
        [HttpGet]
        [ResponseType(typeof(IEnumerable<LocationDto>))]
        public IHttpActionResult GetLocations()
        {
            IEnumerable<Location> locations = _unitOfWork.LocationRepository.GetAll();
            if (locations == null || locations.Count() < 1)
            {
                return NotFound();
            }
            IEnumerable<LocationDto> locatinoDtos = _iMapper.Map<IEnumerable<Location>, IEnumerable<LocationDto>>(locations);
            return Ok(locatinoDtos);
        }

        // GET: api/Locations/5
        [HttpGet]
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult GetLocation(int id)
        {
            Location location = _unitOfWork.LocationRepository.GetById(id);
            if (location == null)
            {
                return NotFound();
            }
            LocationDto locationDto = _iMapper.Map<Location, LocationDto>(location);
            return Ok(locationDto);
        }

        // PUT: api/Locations/5
        [HttpPut]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLocation(int id, LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationDto.Id)
            {
                return BadRequest();
            }

            Location location = _iMapper.Map<LocationDto, Location>(locationDto);

            try
            {
                _unitOfWork.LocationRepository.Add(location);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        [ResponseType(typeof(LocationDto))]
        public IHttpActionResult PostLocation(LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Location location = _iMapper.Map<LocationDto, Location>(locationDto);

            try
            {
                _unitOfWork.LocationRepository.Add(location);
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
            }

            return CreatedAtRoute("DefaultApi", new { id = location.Id }, location);
        }

        [HttpPost]
        [AuthorizationFilter(new string[] { "DRIVER"})]
        [Route("api/locations/addOrUpdateDriverLocation")]
        public IHttpActionResult AddOrUpdateDriverLocation([FromBody]LocationDto newOrUpdatedLocationDto)
        {
            string hash = Thread.CurrentPrincipal.Identity.Name;

			LoginModel loginModel = null;

			if (_accessService.GetLoginData(hash, _unitOfWork) != null)
			{
				loginModel = _accessService.GetLoginData(hash, _unitOfWork).Data;
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			Location newOrUpdatedLocation = _iMapper.Map<LocationDto, Location>(newOrUpdatedLocationDto);

			if (!LocationExists(newOrUpdatedLocation.Id))
			{
				_unitOfWork.LocationRepository.Add(newOrUpdatedLocation);
				_unitOfWork.Complete();

				User driver = _unitOfWork.UserRepository.GetUserByUsernameIncludeAll(loginModel.Username, loginModel.Role);
				driver.DriverLocationId = newOrUpdatedLocation.Id;

				_unitOfWork.UserRepository.Update(driver);
				_unitOfWork.Complete();

				LocationDto newLocationDto = _iMapper.Map<Location, LocationDto>(newOrUpdatedLocation);
				return Ok(newLocationDto);
			}
			else
			{
				Location oldLocation = _unitOfWork.LocationRepository.Find(l => l.Id == newOrUpdatedLocation.Id).FirstOrDefault();
				foreach (PropertyInfo property in typeof(Location).GetProperties())
				{
					if (property.CanWrite)
					{
						property.SetValue(oldLocation, property.GetValue(newOrUpdatedLocation, null), null);
					}
				}
				_unitOfWork.LocationRepository.Update(oldLocation);
				_unitOfWork.Complete();


				User driver = _unitOfWork.UserRepository.GetUserByUsernameIncludeAll(loginModel.Username, loginModel.Role);
				driver.DriverLocationId = newOrUpdatedLocation.Id;

				_unitOfWork.UserRepository.Update(driver);
				_unitOfWork.Complete();

				LocationDto updatedLocationDto = _iMapper.Map<Location, LocationDto>(oldLocation);
				return Ok(updatedLocationDto);
			}
        }

        //// DELETE: api/Locations/5
        //[ResponseType(typeof(Location))]
        //public IHttpActionResult DeleteLocation(int id)
        //{
        //    Location location = db.Locations.Find(id);
        //    if (location == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Locations.Remove(location);
        //    db.SaveChanges();

        //    return Ok(location);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(int id)
        {
            return _unitOfWork.LocationRepository.GetById(id) != null;
        }
    }
}