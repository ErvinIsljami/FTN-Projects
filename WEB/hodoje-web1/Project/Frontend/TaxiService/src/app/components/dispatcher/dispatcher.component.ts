import { DriverFormValidators } from './../../common/validators/driver-form.validators';
import { DispatcherFormRideRequest } from './../../models/dispatcherFormRideRequest';
import { AccessService } from './../../services/access.service';
import { RidesService } from './../../services/rides.service';
import { RegistrationModel } from './../../models/registration.model';
import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../../models/user.model';
import { Car } from '../../models/car.model';
import { Location } from './../../models/location.model';
import { Ride } from '../../models/ride.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { DispatcherProcessRideRequest } from '../../models/dispatcherProcessRideRequest';
import { ApiMessage } from '../../models/apiMessage.model';
import { RideFormValidators } from '../../common/validators/ride-form.validators';
import { MapInfo } from '../../models/map-information.model';

declare var jQuery: any;
declare var google;

@Component({
  selector: 'dispatcher',
  templateUrl: './dispatcher.component.html',
  styleUrls: ['./dispatcher.component.scss']
})
export class DispatcherComponent implements OnInit {

  personalData: User;
  allDrivers: User[];
  allRides: Ride[];
  allUsers: User[];
  closestDrivers: User[];
  dispatcherRides: Ride[];
  pendingRides: Ride[];
  ratingList = [false, false, false, false, false];
  mapInfo: MapInfo;
  rideInProcess: Ride;

  personalDataForm = new FormGroup({
    username: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    password: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    name: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    lastname: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    email: new FormControl(
      null, 
      [Validators.required, Validators.email, Validators.maxLength(254)]
    ),
    gender: new FormControl(
      null,
      Validators.required
    ),
    nationalIdentificationNumber: new FormControl(
      null, 
      [Validators.minLength(13), Validators.maxLength(13),Validators.pattern('[0-9]*')]
    ),
    phoneNumber: new FormControl(
      null, 
      [Validators.minLength(5), Validators.maxLength(10), Validators.pattern('[0-9]*')]
    )
  });

  rideForm = new FormGroup({
    location: new FormGroup({
      address: new FormGroup({
        streetName: new FormControl(
          null,
          [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-z A-Z]*')]
        ),
        streetNumber: new FormControl(
          null,
          [Validators.required, Validators.minLength(1), Validators.maxLength(4), Validators.pattern('[0-9]*')]
        ),
        city: new FormControl(
          null,
          [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-z A-Z]*')]
        ),
        postalCode: new FormControl(
          null,
          [Validators.required, Validators.minLength(1), Validators.maxLength(10), Validators.pattern('[a-zA-Z0-9]*')]
        )
      }),
      longitude: new FormControl(
        null,
        [Validators.required, RideFormValidators.checklongitudeInterval]
      ),
      latitude: new FormControl(
        null,
        [Validators.required, RideFormValidators.checklatitudeInterval]
      )
    }),
    carType: new FormControl(),
    driverId: new FormControl(
      null,
      Validators.required
    )
  });  

  driverForm = new FormGroup({
    username: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    password: new FormControl(
      null, 
      [Validators.required, Validators.minLength(8), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    name: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    lastname: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z]*')]
    ),
    email: new FormControl(
      null, 
      [Validators.required, Validators.email, Validators.maxLength(254)]
    ),
    gender: new FormControl(
      null,
      Validators.required
    ),
    nationalIdentificationNumber: new FormControl(
      null, 
      [Validators.minLength(13), Validators.maxLength(13),Validators.pattern('[0-9]*')]
    ),
    phoneNumber: new FormControl(
      null, 
      [Validators.minLength(5), Validators.maxLength(10), Validators.pattern('[0-9]*')]
    ),
    car: new FormGroup({
      registrationNumber: new FormControl(
        null,
        [Validators.required, Validators.pattern('[a-zA-Z0-9]*')]
      ),
      taxiNumber: new FormControl(
        null,
        [Validators.required, Validators.pattern('[0-9]*'), DriverFormValidators.checkTaxiNumberInterval]
      ),
      yearOfManufactoring: new FormControl(
        null,
        [Validators.required, Validators.pattern('[0-9]*'), DriverFormValidators.checkYearOfManufactoringInterval]
      ),
      carType: new FormControl(
        null,
        Validators.required
      )
    })
  });

  processARideForm = new FormGroup({
    driverId: new FormControl(
      null,
      Validators.required
    )
  });

  dispatcherRidesSearchForm = new FormGroup({
    userType: new FormControl(),
    name: new FormControl(),
    lastname: new FormControl()
  });

  constructor(private usersService: UsersService, private ridesService: RidesService, private accessService: AccessService) {
  }

  ngOnInit() {
    this.mapInfo = new MapInfo(45.246102512788326, 19.851694107055664);
    this.personalData = new User();
    this.allDrivers = [];
    this.allRides = [];
    this.allUsers = [];
    this.closestDrivers = [];
    this.pendingRides = [];
    this.dispatcherRides = [];
    this.rideInProcess = new Ride();
    this.getMyData();
    this.getAllRides();
    this.getAllUsers();
    this.getAllDrivers();
    this.getAllDispatcherRides();
    this.getAllPendingRides();
  }

  get pdForm(){
    return this.personalDataForm.controls;
  }
  
  get rdForm(){
    return this.rideForm.controls;
  }

  get drForm(){
    return this.driverForm.controls;
  }

  get parForm(){
    return this.processARideForm.controls;
  }

  get passengerDrivers(){
    return this.allDrivers.filter(d => d.car.carType === 'PASSENGER');
  }

  get vanDrivers(){
    return this.allDrivers.filter(d => d.car.carType === 'VAN');
  }

  rideFormPlaceMarker(event, rideForm: FormGroup){
    this.mapInfo.lat = event.coords.lat;
    this.mapInfo.long = event.coords.lng;

    let resultLocationData: any;
    let geocoder = new google.maps.Geocoder();
    let mylatLng = new google.maps.LatLng(this.mapInfo.lat, this.mapInfo.long);
    let geocoderRequest = {latLng: mylatLng};
    geocoder.geocode(geocoderRequest, function(result, status){
      let rideLocation = new Location();
      resultLocationData = result[0] as Array<string>;

      rideLocation.latitude = event.coords.lat;
      rideLocation.longitude = event.coords.lng;

      resultLocationData.address_components.forEach(prop => {
        if(prop.types.find(t => t === 'street_number') !== undefined){
          rideLocation.address.streetNumber = prop.long_name as string;
        }
        if(prop.types.find(t => t === 'route') !== undefined){
          rideLocation.address.streetName = prop.long_name as string;
          
        }
        if(prop.types.find(t => t === 'locality') !== undefined){
          rideLocation.address.city = prop.long_name as string;
          
        }
        if(prop.types.find(t => t === 'postal_code') !== undefined){
          rideLocation.address.postalCode = prop.long_name as string;
        }
      });

      rideForm.patchValue({
        location: rideLocation
      });
    });
  }

  getClosestDrivers(){
    let longitude = this.rideInProcess.startLocation.longitude;
    let latitude = this.rideInProcess.startLocation.latitude;

    this.allDrivers.sort((a, b) => 
      (Math.abs(a.driverLocation.longitude - longitude) + Math.abs(a.driverLocation.latitude - latitude)) - (Math.abs(b.driverLocation.longitude - longitude) + Math.abs(b.driverLocation.latitude - latitude))
    );

    let closestDrivers = [];
    for(var i = 0; i < 5; i++){
      closestDrivers.push(this.allDrivers[i]);
    }
    return closestDrivers;
  }

  getMyData(){
    this.usersService.getUserByUsername().subscribe(
    (data: User) =>{
      this.personalData = data;
      this.personalDataForm.patchValue({
        username: data.username,
        password: data.password,
        name: data.name,
        lastname: data.lastname,
        email: data.email,
        gender: data.gender,
        nationalIdentificationNumber: data.nationalIdentificationNumber,
        phoneNumber: data.phoneNumber
      });
      this.personalDataForm.markAsPristine();
    });
  }

  changeMyData(){
    let updatedUser = new User();
    updatedUser =  this.personalDataForm.value;
    updatedUser.id = this.personalData.id;
    updatedUser.isBanned = this.personalData.isBanned;
    updatedUser.role = this.personalData.role;

    let apiMessage = new ApiMessage(localStorage.userHash, updatedUser);

    this.usersService.put(updatedUser.id, apiMessage).subscribe(
      (data: string) => {
        localStorage.userHash = data;
        this.personalDataForm.markAsPristine();
      },
      error => {
        console.log(error);
      }
    );
  }

  private parseSingleRide(unparsedRide: Ride){
    let parsedRide = unparsedRide;
    let tempdate = new Date(unparsedRide.timestamp);
    parsedRide.timestamp = `${tempdate.toLocaleDateString()} ${tempdate.toLocaleTimeString()}`;
    if(parsedRide.comments.length > 0){
      parsedRide.comments.forEach(c => {
        c.timestamp = new Date(c.timestamp);  
      })
    }
    return parsedRide;
  }

  private parseRides(unparsedRides: Ride[]){
    let parsedRides = unparsedRides;
    parsedRides.forEach(r => {
      r = this.parseSingleRide(r);
    });
    return parsedRides;
  }

  getAllRides(){
    this.ridesService.getAllRides().subscribe(
      (data: Ride[]) => {
        this.allRides = this.parseRides(data);
      }
    );
  }

  getAllPendingRides(){
    this.ridesService.getAllPendingRides().subscribe(
      (data: Ride[]) => {
        this.pendingRides = this.parseRides(data);
      }
    );
  }

  getAllDispatcherRides(){
    this.ridesService.getAllDispatcherRides().subscribe(
      (data: Ride[]) => {
        this.dispatcherRides = this.parseRides(data);
      }
    );
  }

  getAllUsers(){
    this.usersService.getAllUsers().subscribe(
      (data: User[]) => {
        this.allUsers = data;
      }
    );
  }

  getAllDrivers(){
    this.usersService.getAllDrivers().subscribe(
      (data: User[]) => {
        this.allDrivers = data;
      }
    );
  }

  addDriver(){
    let newDriver = new User();
    newDriver = this.driverForm.value;
    newDriver.role = 'DRIVER';
    console.log(newDriver);
    this.usersService.post(newDriver).subscribe(
      (data: Location) => {
        console.log('matori')
        jQuery("#addADriverModal").modal("toggle");
        this.driverForm.reset();
        this.getAllUsers();
      }
    );
  }

  blockUser(usernameToBlock){
    this.accessService.blockUser(usernameToBlock).subscribe(
      () => {
        let driver = this.allUsers.find(u => u.username === usernameToBlock);
        driver.isBanned = true;
      }
    );
  }

  unblockUser(usernameToUnblock){
    this.accessService.unblockUser(usernameToUnblock).subscribe(
      () => {
        let driver = this.allUsers.find(u => u.username === usernameToUnblock);
        driver.isBanned = false;
      }
    );
  }

  searchRides(){
    this.ridesService.dispatcherRidesSearch(this.dispatcherRidesSearchForm.value).subscribe(
      (data: Ride[]) => {
        this.allRides = this.parseRides(data);
      }
    );
  }

  formARide(){
    let formRideRequest = new DispatcherFormRideRequest();
    formRideRequest = this.rideForm.value;
    formRideRequest.dispatcherId = this.personalData.id;
    this.ridesService.formARide(formRideRequest).subscribe(
      () => {
        jQuery("#formARideModal").modal("toggle");
        this.rideForm.reset();
        this.getAllRides();
        this.getAllDispatcherRides();
      }
    );
  }

  startProcessingARide(rideId){
    let selectedRide = this.pendingRides.find(r => r.id === rideId);
  }

  processARide(rideId){
    jQuery("#processARideModal").modal("toggle");
    let processARideRequest = new DispatcherProcessRideRequest();
    processARideRequest = this.processARideForm.value;
    processARideRequest.dispatcherId = this.personalData.id;
    processARideRequest.rideId = rideId;
    this.ridesService.processARide(processARideRequest).subscribe(
      () => {
        this.getAllRides();
        this.getAllDispatcherRides();
        this.getAllPendingRides();
      }
    );
  }
}
