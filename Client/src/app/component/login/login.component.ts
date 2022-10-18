import { Component, OnInit } from '@angular/core';
import { Login } from 'src/app/model/login';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/service/login/userService';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginRequest: Login = new Login()
  signIn: User = new User()
  constructor( private userService: UserService) { }
  ngOnInit(): void {
    this.loginRequest = new Login()
  }
  onSubmit(): void {
    this.userService.login(this.loginRequest)
    this.loginRequest = new Login()
    // window.location.reload()
  }

  register = (files:any) => {
    if (files.length === 0) {return;}
    this.userService.signup(this.signIn,files);
    this.signIn = new User();
  }
}





