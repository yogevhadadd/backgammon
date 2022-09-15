import { Injectable } from '@angular/core';
import { Token } from '../../interface/Token';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  constructor() { }

  saveSession(tokenResponse: Token) {
    window.localStorage.setItem('jwt', tokenResponse.accessToken);
    window.localStorage.setItem('NickName', tokenResponse.nickName);
    
  }
  getSession(): Token | null {
    if (window.localStorage.getItem('AT')) {
      const tokenResponse: Token = {
        accessToken: window.localStorage.getItem('AccessToken') || '',
        nickName: window.localStorage.getItem('NickName') || '',
        refreshToken: window.localStorage.getItem('RefreshToken') || ''
      };
      return tokenResponse;
    }
    return null;
  }
}
