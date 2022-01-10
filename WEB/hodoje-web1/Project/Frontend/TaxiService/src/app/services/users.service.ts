import { ApiMessage } from './../models/apiMessage.model';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { GenericService } from './generic.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'users', httpClient);
  }

  getUserByUsername(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/users/getuserbyusername', {'headers': headers});
  }

  getAllDrivers(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/users/getalldrivers', {'headers': headers});
  }

  getAllUsers(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/users/getallusers', {'headers': headers});
  }
}
