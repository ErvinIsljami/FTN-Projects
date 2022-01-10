import { Location } from "./location.model";

export class DispatcherFormRideRequest{
  location: Location;
  carType: string;
  driverId: number;
  dispatcherId: number;

  constructor(){
    this.location = new Location();
  }
}