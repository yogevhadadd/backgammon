import { Component, OnInit } from '@angular/core';
import { Login } from 'src/app/model/login';
import { UserService } from 'src/app/service/login/userService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginRequest: Login = new Login()
  constructor( private userService: UserService) { }
  ngOnInit(): void {
    this.loginRequest = new Login()
  }
  onSubmit(): void {
    this.userService.login(this.loginRequest)
    this.loginRequest = new Login()
    // window.location.reload()
  }
}





