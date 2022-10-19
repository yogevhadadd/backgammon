import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DisplayName } from 'src/app/model/displayName';
import { Contact } from 'src/app/model/player';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  constructor(private http: HttpClient) { }
  readonly baseUrl = "https://localhost:7088/Home"
  displayName:DisplayName = new DisplayName();
  getContact(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.baseUrl);
  }
  getChat(displayName: string){
    this.displayName.displayName = displayName;
    this.http.post(this.baseUrl+ '/PostChat', this.displayName, {
      headers: new HttpHeaders({ "Content-Type": "application/json"})
    }).subscribe();

  }
}
