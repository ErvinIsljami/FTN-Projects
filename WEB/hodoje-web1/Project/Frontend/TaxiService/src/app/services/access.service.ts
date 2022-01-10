import { ApiMessage } from '../models/apiMessage.model';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AccessService{

  url: string;
  endpoint: string;
  constructor(private httpClient: HttpClient) {
    this.url = 'http://localhost:3737/api';
    this.endpoint = 'access';
  }

  login(loginRequest: ApiMessage){
    return this.httpClient.post(`${this.url}/${this.endpoint}/login`, loginRequest);
  }

  logout(logoutRequest: ApiMessage){
    return this.httpClient.post(`${this.url}/${this.endpoint}/logout`, logoutRequest);
  }

  blockUser(usernameToBlock: string){
    let headers = new HttpHeaders();
    headers = headers.append('Content-type', 'text/plain; charset=utf-8');
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post(`${this.url}/${this.endpoint}/blockuser`, usernameToBlock, {'headers': headers});
  }

  unblockUser(usernameToUnblock: string){
    let headers = new HttpHeaders();
    headers = headers.append('Content-type', 'text/plain; charset=utf-8');
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post(`${this.url}/${this.endpoint}/unblockuser`, usernameToUnblock, {'headers': headers});
  }
}
