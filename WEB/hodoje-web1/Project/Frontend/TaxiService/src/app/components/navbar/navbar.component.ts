import { ApiMessage } from './../../models/apiMessage.model';
import { AccessService } from '../../services/access.service';
import { Component, OnInit } from '@angular/core';
import { LoginToNavbarService } from '../../services/login-to-navbar.service';

@Component({
  selector: 'navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit{
  
  isLoggedIn = false;

  constructor(private loginToNavbarService: LoginToNavbarService, private accessService: AccessService) { }

  ngOnInit(){
    this.loginToNavbarService.change.subscribe(
      isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
      }
    );
    if(localStorage.userHash === "null"){
      this.isLoggedIn = false;
    }
    else{
      this.isLoggedIn = true;
    }
  }

  logout(){
    let apiRequest = new ApiMessage(localStorage.userHash, null);
    this.accessService.logout(apiRequest).subscribe(
      (data: ApiMessage) => {
        if(localStorage.userHash === data.key){
          localStorage.userHash = null;
          localStorage.role = null;
          this.isLoggedIn = false;
        }
      },
      error => {
        localStorage.userHash = null;
        localStorage.role = null;
        this.isLoggedIn = false;
      }
    );
  }
}
