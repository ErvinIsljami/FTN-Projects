import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ContentGuard implements CanActivate {

  constructor(private router: Router){}

  canActivate(){
    let userHash = localStorage.userHash;
    if(userHash !== 'null'){
      return true;
    }
    else{
      this.router.navigate(['/login']);
      return false;
    }
  }
}
