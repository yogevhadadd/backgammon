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

    
    this._hubConnection.on('ReceiveMessage', (data: any) => {
        this.messages = data;
    });
    
    this._hubConnection.on('SendChat', (data: any,firstPic:string, name:string) => {
        this.againtsUser = name;
        this.firstPic = firstPic;
        this.messages = data;
    });
  }

  public async sendMessage(msg: string, displayName: string) {
    this._hubConnection?.invoke("SendMessage", msg, displayName);
  }
  public async getChat(displayName: string,img: string) {
    this._hubConnection?.invoke("GetChat",displayName,img);
  }
  
  public async SendGameRequest(displayName: string) {
    this._hubConnection?.invoke("SendGameRequest",displayName);
  }
}