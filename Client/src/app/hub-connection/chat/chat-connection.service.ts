import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ChatMessage } from '../../model/chatMessage';

@Injectable({
  providedIn: 'root'
})
export class ChatConnectionService {
  private _hubConnection!: HubConnection;
  public messages:ChatMessage[] = [];
  public firstPic:string = "";
  public againtsUser:string = "";
  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }
  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
      .withUrl('https://localhost:44347/chat',{
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
    this._hubConnection.on('ReceiveMessage', (data: any,firstPic:string, secondPic:string,firstName:string,secondName:string) => {
      if(window.localStorage.getItem('NickName') == firstName){
          this.firstPic = secondPic;
          this.againtsUser = secondName;
      }
      else{
        this.firstPic = firstPic;
        this.againtsUser = firstName;
      }
        this.messages = data;
    });
  }
  public async sendMessage(msg: string) {
    this._hubConnection?.invoke("SendMessage", msg);
  }
}