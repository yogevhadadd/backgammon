import { Component, Input, OnInit } from '@angular/core';
import { ChatConnectionService } from 'src/app/hub-connection/chat/chat-connection.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  @Input() game:string = "";
  @Input() chatUser = "";

  constructor(public _hubChat: ChatConnectionService) { console.log("Sdcsdc")}
  user:any = "";
  ngOnInit(): void {
    this.user = localStorage.getItem('NickName')
    console.log(this._hubChat.firstPic)
  }
  public sendMessage(message: string)  {
    if(message != ""){
      this._hubChat.sendMessage(message,this.chatUser);
    }
  }
}