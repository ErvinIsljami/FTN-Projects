export class CancelRideRequest{
  cancel: boolean;

  constructor(shouldCancel: boolean){
    this.cancel = shouldCancel;
  }
}