import { Comment } from './comment.model';
import { User } from './user.model';
import { RideStatus } from "./rideStatus";

export class ChangeRideRequest{
  location: Location;
  carType: string;

  constructor(){
    this.location = new Location();
  }
}