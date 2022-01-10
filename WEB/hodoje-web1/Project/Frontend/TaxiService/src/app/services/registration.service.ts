import { Injectable } from '@angular/core';
import { RegistrationModel } from '../models/registration.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  url: string;
  endpoint: string;
  constructor(private httpClient: HttpClient) {
    this.url = 'http://localhost:3737/api';
    this.endpoint = 'registration';
  }

  register(registrationModel: RegistrationModel){
    return this.httpClient.post(`${this.url}/${this.endpoint}`, registrationModel);
  }
}
