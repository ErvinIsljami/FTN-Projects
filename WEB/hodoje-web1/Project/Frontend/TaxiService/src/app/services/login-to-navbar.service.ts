import { Injectable, Output } from '@angular/core';
import { EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoginToNavbarService {

  isLoggedIn = false;

  @Output() change: EventEmitter<boolean> = new EventEmitter();
  
  login(){
    this.change.emit(true);
  }
}
