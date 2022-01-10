import { DriverComponent } from '../driver/driver.component';
import { CustomerComponent } from '../customer/customer.component';
import { DispatcherComponent } from '../dispatcher/dispatcher.component';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.scss']
})
export class ContentComponent implements OnInit {

  role: string;

  constructor() { }

  ngOnInit() {
    this.role = (localStorage.role as string).toLowerCase();
  }

}
