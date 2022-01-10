import { RegistrationModel } from './../../models/registration.model';
import { Component } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';

@Component({
  selector: 'registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent{

  genders: string[];
  registrationSuccessful: boolean;
  constructor(private registrationService: RegistrationService) {
    this.genders = ['MALE', 'FEMALE'];
    this.registrationSuccessful = false;
  }

  register(registrationModel: RegistrationModel){
    this.registrationService.register(registrationModel).subscribe(
      data => {
        this.registrationSuccessful = true;
      },
      error => {
        console.log(error);
      }
    );
  }
}
