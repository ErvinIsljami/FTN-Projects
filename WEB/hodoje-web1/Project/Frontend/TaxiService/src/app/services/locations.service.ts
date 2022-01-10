import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GenericService } from './generic.service';
import { Injectable } from '../../../node_modules/@angular/core';
import { Location } from '../models/location.model';

@Injectable({
  providedIn: 'root'
})
export class LocationsService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'locations', httpClient);
  }

  addOrupdateDriverLocation(location: Location){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/locations/addOrUpdateDriverLocation', location, {'headers': headers});
  }
}
