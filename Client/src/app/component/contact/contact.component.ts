import { Component,  OnInit } from '@angular/core';
import { ChatConnectionService } from 'src/app/hub-connection/chat/chat-connection.service';
import { ContactConnectionService } from 'src/app/hub-connection/contact/contact-connection.service';
import { Contact } from 'src/app/model/player';
import { ContactService } from 'src/app/service/contact/contact.service';

@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {

  constructor(public _hubChat: ChatConnectionService ,private  _ContactService: ContactService
              ,public _hubContact: ContactConnectionService){

    this.myUser = localStorage.getItem('NickName');
    this._hubContact.Start()
  }
  allContact: Contact[] = [];
  onlineScreen: boolean = true;
  myUser:any = "";
  chatUser:any = "";
  ngOnInit(): void {
  }
  Chat(displayName:string,img: string){
    this._hubChat.getChat(displayName,img);
    this.chatUser = displayName;
  }
}
