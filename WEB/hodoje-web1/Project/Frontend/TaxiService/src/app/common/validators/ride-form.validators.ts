import { AbstractControl, ValidationErrors } from "@angular/forms";

export class RideFormValidators{
  static checklongitudeInterval(control: AbstractControl) : ValidationErrors | null {
    let longitude = control.value as number;
    if(longitude > 180 || longitude < -180){
      return { isNotInLongitudeInterval : {
        minLongitude: -180,
        maxLongitude: 180
      }};
    }
    else{
      return null;
    }
  }

  static checklatitudeInterval(control: AbstractControl) : ValidationErrors | null {
    let latitude = control.value as number;
    if(latitude > 90 || latitude < -90){
      return { isNotInLatitudeInterval: {
        minLatitude: -90,
        maxLatitude: 90
      }};
    }
    else{
      return null;
    }
  }
}