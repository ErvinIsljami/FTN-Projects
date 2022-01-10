import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

// @Injectable({
//   providedIn: 'root'
// })
export abstract class GenericService {

  constructor(private url: string, private endpoint: string, protected httpClient: HttpClient) { }

  public getById(id: number){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get(`${this.url}/${this.endpoint}/${id}`, {'headers': headers});
  }

  public getAll(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get(`${this.url}/${this.endpoint}`, {'headers': headers});
  }

  public post(item: any){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post(`${this.url}/${this.endpoint}`, item, {'headers': headers});
  }

  public put(id: number, item: any){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.put(`${this.url}/${this.endpoint}/${id}`, item, {'headers': headers});
  }

  public delete(id: number){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.delete(`${this.url}/${this.endpoint}/${id}`, {'headers': headers});
  }
}
