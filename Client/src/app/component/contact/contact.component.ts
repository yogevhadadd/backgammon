import { Component,  OnInit } from '@angular/core';
import { ChatConnectionService } from 'src/app/hub-connection/chat/chat-connection.service';
import { ContactConnectionService } from 'src/app/hub-connection/contact/contact-connection.service';
import { GameConnectionService } from 'src/app/hub-connection/game/game-connection.service';
import { Contact } from 'src/app/model/player';
import { ContactService } from 'src/app/service/contact/contact.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  chatOrGame:boolean = true;
  onlineScreen: boolean = true;
  myUser:any = "";
  chatUser:any = "Chat";
  constructor(public _hubChat: ChatConnectionService ,private _hubGame:GameConnectionService
              ,public _hubContact: ContactConnectionService){
    this.myUser = localStorage.getItem('NickName');
    this._hubContact.Start()
  }
  
  ngOnInit(): void {

  }
  Chat(displayName:string,img: string){
    this._hubChat.getChat(displayName,img);
    this._hubGame.StartNewGame(displayName)
    this.chatUser = displayName;
  }
  public ChangGame(){
    this.chatOrGame = !this.chatOrGame;
  }
}
