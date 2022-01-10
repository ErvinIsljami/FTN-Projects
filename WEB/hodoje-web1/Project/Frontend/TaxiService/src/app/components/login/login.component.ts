import { Component, Output, EventEmitter } from '@angular/core';
import { AccessService } from '../../services/access.service';
import { LoginModel } from '../../models/login.model';
import { ApiMessage } from '../../models/apiMessage.model';
import { Router } from '@angular/router';
import { LoginToNavbarService } from '../../services/login-to-navbar.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent{

  isLoggedIn: boolean;
  apiRequest: ApiMessage;
  isBadLoginParams: boolean;
  isOtherError: boolean;
  
  constructor(
    private accessService: AccessService, 
    private router: Router, 
    private loginToNavbarService: LoginToNavbarService
  ) { }

  login(loginModel: LoginModel){
    this.isBadLoginParams = false;
    this.apiRequest = new ApiMessage("null", loginModel);

    this.accessService.login(this.apiRequest).subscribe(
      (data: ApiMessage) => {
        localStorage.setItem('userHash', data.key);
        localStorage.setItem('role', data.data.role);
        this.isLoggedIn = true;
        this.router.navigate(['/profile']);

        this.loginToNavbarService.login();
      },
      error => {
        if(error.status === 400){
          this.isBadLoginParams = true;
        }
        else{
          this.isOtherError = true;
        }
        console.log(error);
      }
    );
  }

  logout(){
    this.apiRequest = new ApiMessage(localStorage.userHash, null);
    this.accessService.logout(this.apiRequest).subscribe(
      (data: ApiMessage) => {
        if(localStorage.userHash === data.key){
          localStorage.userHash = null;
          localStorage.role = null;
          this.isLoggedIn = false;

          this.loginToNavbarService.login();
        }
      },
      error => {
        localStorage.userHash = null;
        localStorage.role = null;
        this.isLoggedIn = false;
        console.log(error)
      }
    );
  }
}
