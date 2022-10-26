import { Component, Input, OnInit } from '@angular/core';
import { ChatConnectionService } from 'src/app/hub-connection/chat/chat-connection.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  @Input() game:string = "";
  @Input() chatUser = "Chat";
  @Input() chatOrGame:boolean = true;
  constructor(public _hubChat: ChatConnectionService) {}
  user:any = "";
  ngOnInit(): void {
    this.user = localStorage.getItem('NickName')
  }
  public sendMessage(message: string)  {
    if(message != ""){
      this._hubChat.sendMessage(message,this.chatUser);
    }
  }
  public ChangGame(){
    this.chatOrGame = !this.chatOrGame;
    
  }
  public SendGameRequest(){
    this._hubChat.SendGameRequest(this.chatUser)
  }
}