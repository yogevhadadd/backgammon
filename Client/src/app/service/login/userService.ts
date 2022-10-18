import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Token } from '../../interface/Token';
import { User } from '../../model/user';
import { Login } from 'src/app/model/login';
import { Router } from '@angular/router';
import { ProfilePic } from 'src/app/model/profilePic';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl: string =  'https://localhost:7048/LoginRegistar'
  profile:ProfilePic|undefined = new ProfilePic();
  constructor(private router: Router, private httpClient: HttpClient) { }

  async signup(SignupRequest: User,files:any) {
    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    await this.httpClient.post<ProfilePic>(this.apiUrl + '/uploadImg', formData).toPromise().then(data => {
      SignupRequest.ProfilePic = data?.profilePic
      this.profile = data;
      });
    return  this.httpClient.post<Token>(this.apiUrl + "/Registar", SignupRequest, {
      headers: new HttpHeaders({ "Content-Type": "application/json"})
      }).subscribe({
        next: (response: Token) => {
          this.LoginSuccess(response)
          this.router.navigate(["/contact"]);
        }
    })
  }
  login(loginRequest: Login) {
      return this.httpClient.post<Token>(this.apiUrl + '/login', loginRequest, {
        headers: new HttpHeaders({ "Content-Type": "application/json"})
      }).subscribe({
        next: (response: Token) => {
          this.LoginSuccess(response)
          this.router.navigate(["/contact"]);
        }
    })
  }
  getUsers(){
    return this.httpClient.get<User[]>(this.apiUrl);
  }
  private LoginSuccess(response: Token){
    var accessToken = response.accessToken
    var nickName = response.nickName
    var refreshToken = response.refreshToken;
    window.localStorage.setItem('jwt', accessToken);
    window.localStorage.setItem('NickName', nickName);
    window.localStorage.setItem('refreshToken', refreshToken);
  }
}
