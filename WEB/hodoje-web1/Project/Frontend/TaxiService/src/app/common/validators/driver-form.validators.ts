import { ValidationErrors } from '@angular/forms';
import { AbstractControl } from '@angular/forms';
export class DriverFormValidators{
  static checkTaxiNumberInterval(control: AbstractControl) : ValidationErrors | null {
    let taxiNumber = control.value as number;
    if(taxiNumber > 1000000 || taxiNumber < 0){
      return { isNotInTaxiNumberInterval: {
        minTaxiNumber: 0,
        maxTaxiNumber: 1000000
      }};
    }
    else{
      return null;
    }
  }

  static checkYearOfManufactoringInterval(control: AbstractControl) : ValidationErrors | null {
    let yearOfManufactoring = control.value as number;
    if(yearOfManufactoring > 2018 || yearOfManufactoring < 1900){
      return { isNotInYearOfManufactoringInterval: {
        minYearOfManufactoring: 1900,
        maxYearOfManufactoring: 2018
      }};
    }
    else{
      return null;
    }
  }
}