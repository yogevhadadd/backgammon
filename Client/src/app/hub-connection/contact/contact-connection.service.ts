import { Injectable} from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Contact } from 'src/app/model/player';
@Injectable({
  providedIn: 'root'
})
export class ContactConnectionService {
  private _hubConnection!: HubConnection;
  listContact:Contact[]= [new Contact()];
  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }
  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:7088/request',{
        withCredentials: false,
        accessTokenFactory: () => localStorage.getItem('jwt')!,
      })
      .build();
  }
  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
      })
      .catch(err => {
        setTimeout( () => { this.startConnection(); }, 5000);
      });
  }
  private registerOnServerEvents(): void {
    this._hubConnection.on('SendListConnect', (listConnect: any) => {
        this.listContact = listConnect;
        console.log(this.listContact)
        console.log("dfghjhgfdsdfghjkjhgfdsdfghjkkkkkkhhhhhhhhhhhhhhhhh")
    });
  }
  public  async Start() {
    this._hubConnection?.invoke("GetListConnect");
  }
}