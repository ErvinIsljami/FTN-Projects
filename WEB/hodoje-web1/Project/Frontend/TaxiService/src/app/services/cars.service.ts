import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GenericService } from './generic.service';
import { Car } from '../models/car.model';

@Injectable({
  providedIn: 'root'
})
export class CarsService extends GenericService{

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'cars', httpClient);
  }

  updateCar(updatedCar: Car){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.put(`http://localhost:3737/api/cars/updateCar/${updatedCar.id}`, updatedCar, {'headers': headers});
  }
}
