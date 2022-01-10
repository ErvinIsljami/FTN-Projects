import { Comment } from './../models/comment.model';
import { DispatcherProcessRideRequest } from './../models/dispatcherProcessRideRequest';
import { DispatcherFormRideRequest } from './../models/dispatcherFormRideRequest';
import { RefineRidesModel } from './../models/refine.model';
import { ApiMessage } from './../models/apiMessage.model';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { GenericService } from './generic.service';
import { RideRequest } from '../models/rideRequest';
import { ChangeRideRequest } from '../models/changeRideRequest';
import { CancelRideRequest } from '../models/cancelRideRequest';
import { Ride } from '../models/ride.model';

@Injectable({
  providedIn: 'root'
})
export class RidesService extends GenericService {

  constructor(httpClient: HttpClient) {
    super('http://localhost:3737/api', 'rides', httpClient);
  }

  getAllMyRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllMyRides', {'headers' : headers});
  }

  getAllRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllRides', {'headers' : headers});
  }

  getAllPendingRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllPendingRides', {'headers': headers});
  }

  getAllDispatcherRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllDispatcherRides', {'headers' : headers});
  }

  getAllDriverRides(){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.get('http://localhost:3737/api/rides/getAllDriverRides', {'headers': headers});
  }

  requestRide(rideRequestData: RideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/rideRequest', rideRequestData, {'headers': headers});
  }

  changeRide(changeRideRequestData: ChangeRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/changeRideRequest', changeRideRequestData, {'headers': headers});
  }

  cancelRide(cancelRideRequest: CancelRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));    
    return this.httpClient.post('http://localhost:3737/api/rides/cancelRideRequest', cancelRideRequest, {'headers': headers});
  }  

  addComment(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/addComment', comment, {'headers': headers});
  }

  commentCancelledRide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/commentLatestCancelledRide', comment, {'headers': headers});
  }

  rateARide(comment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/rateARide', comment, {'headers': headers})
  }

  refine(refine){
    let headers = new HttpHeaders();
    headers = headers.append('Content-type', 'application/json');
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/refine', refine, {'headers': headers});
  }

  dispatcherRidesSearch(searchParams){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherRidesSearch', searchParams, {'headers': headers});
  }

  formARide(dispatcherFormRideRequest: DispatcherFormRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherFormRide', dispatcherFormRideRequest, {'headers': headers});
  }

  processARide(dispatcherProcessRideRequest: DispatcherProcessRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/dispatcherProcessRide', dispatcherProcessRideRequest, {'headers': headers});
  }

  // Using the same model as above because we're sending the same data
  driverTakeOverRide(driverTakeOverRequest: DispatcherProcessRideRequest){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/driverTakeOverRide', driverTakeOverRequest, {'headers': headers});
  }

  finishFailedRide(driverComment: Comment){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/finishFailedRide', driverComment, {'headers': headers});
  }

  finishSuccessfulRide(successfulRide: Ride){
    let headers = new HttpHeaders();
    headers = headers.append('Access-Control-Allow-Credentials', 'true');
    headers = headers.append('Authorization', 'Basic ' + btoa(encodeURIComponent(`${localStorage.userHash}`)));
    return this.httpClient.post('http://localhost:3737/api/rides/finishSuccessfulRide', successfulRide, {'headers': headers});
  }
}
