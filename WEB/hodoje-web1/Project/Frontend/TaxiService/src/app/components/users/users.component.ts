import { UsersService } from './../../services/users.service';
import { Component, OnInit } from '@angular/core';

class Nesto{
  id: number; 
  username: string; 
  password: string;
}

@Component({
  selector: 'users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {

  constructor(private usersService: UsersService) { }
  user: Nesto;
  users: Nesto[];

  ngOnInit() {
    this.getAll();
    this.getById(1);
  }

  getAll(){
    return this.usersService.getAll().subscribe(
      data => {
        this.users = data as Nesto[];
      },
      error =>{
        alert(error);
      }
    );
  }

  getById(id: number){
    return this.usersService.getById(id).subscribe(
      data => {
        this.user = data as Nesto;
      },
      error => {
        alert(error);
      });
  }
}
