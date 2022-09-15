import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Contact } from 'src/app/model/player';

@Injectable({
  providedIn: 'root'
})
export class ContactService {
  constructor(private http: HttpClient) { }
  readonly baseUrl = "https://localhost:7088/Home"

  getContact(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.baseUrl);
  }
}
