import { Location } from './location.model';
export class RideRequest {
  location: Location;
  carType: string;

  constructor(){
    this.location = new Location();
  }
}