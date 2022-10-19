import { Injectable} from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Contact } from 'src/app/model/player';
@Injectable({
  providedIn: 'root'
})
export class ContactConnectionService {
  private _hubConnection!: HubConnection;
  listContact:Contact[]= [];
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
    });
    this._hubConnection.on('ReceiveGameRequest', (againtsPlayer:any) => {
          if (confirm(againtsPlayer +" invite you to game") == true) {
            this._hubConnection?.invoke("AcceptRequestGame", againtsPlayer);
          } 
    });
    this._hubConnection.on('AcceptGameRequest', (myPlayer: any) =>{
        window.open("http://localhost:4200/game")
    })
  }
 
  // public async sendMessageRequest(user: string) {
  //   this._hubConnection?.invoke("SendChatRequest", user);
  // }
  // public  async sendGameRequest(user: string) {
  //   this._hubConnection?.invoke("SendGameRequest", user);
  // }
  public  async Start() {
    this._hubConnection?.invoke("GetListConnect");
  }
}