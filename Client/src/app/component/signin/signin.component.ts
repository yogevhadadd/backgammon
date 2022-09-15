import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/service/login/userService';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})

export class SigninComponent implements OnInit {
  user:User = new User();
  isSignUpFailed = false;
  errorMessage = '';
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private userService: UserService) { }
  ngOnInit(): void {}
  
  onSubmit = (files:any) => {
    if (files.length === 0) {return;}
    this.userService.signup(this.user,files);
    this.user = new User();
  }
  
}