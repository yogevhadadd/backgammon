import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Token } from '@angular/compiler';
import { Injectable } from '@angular/core';
import {  CanActivate, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate  {
  refreshRes:any;
  constructor(private router:Router, private jwtHelper: JwtHelperService, private http: HttpClient){}

  async canActivate(): Promise<boolean> {
    const token = localStorage.getItem("jwt");
    if (token && !this.jwtHelper.isTokenExpired(token)){
      console.log(this.jwtHelper.decodeToken(token))
      return true;
    }
    if(token != null){
      const isRefreshSuccess = await this.tryRefreshingTokens(token); 
      if (!isRefreshSuccess) { 
        this.router.navigate(["login"]); 
      }
      return isRefreshSuccess;
    }
    return false;
  }
  private async tryRefreshingTokens(token: string): Promise<boolean> {
    const refreshToken: string|null = localStorage.getItem("refreshToken");
    if (!token || !refreshToken) { 
      return false;
    }
    const credentials = JSON.stringify({ accessToken: token, refreshToken: refreshToken, NickName: localStorage.getItem("NickName")});
    let isRefreshSuccess: boolean;
     this.refreshRes = await new Promise<Token>((resolve, reject) => {
      this.http.post<Token>("https://localhost:7048/LoginRegistar/refresh", credentials, {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe({
        next: (res: Token) => resolve(res),
        error: (_) => { reject; isRefreshSuccess = false;}
      });
    });
    localStorage.setItem("jwt", this.refreshRes.accessToken);
    localStorage.setItem("refreshToken", this.refreshRes.refreshToken);
    isRefreshSuccess = true;
    return isRefreshSuccess;
  }
}
