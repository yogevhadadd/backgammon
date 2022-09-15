import { Component, OnInit } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ContactConnectionService } from 'src/app/hub-connection/contact/contact-connection.service';
import { Contact } from 'src/app/model/player';
import { ContactService } from 'src/app/service/contact/contact.service';
@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',
  styleUrls: ['./contact.component.css']
})
export class ContactComponent implements OnInit {
  constructor(private  _ContactService: ContactService,public _hubContact: ContactConnectionService,private jwtHelper: JwtHelperService){}
  allContact: Contact[] = [];
  onlineScreen: boolean = true;
  myUser:any = "";
  ngOnInit(): void {
    this.getContactOffline();
  }
  SendRequestMessage(nickNameContact:string){
    this._hubContact.sendMessageRequest(nickNameContact);
  }
  SendRequestGame(nickNameContact:string){
    this._hubContact.sendGameRequest(nickNameContact);
  }
  isUserAuthenticated = (): boolean => {
    if((localStorage.getItem('NickName') != undefined && localStorage.getItem('NickName') != "") && this.myUser == ""){ //first time user is Authenticated
      this.myUser = localStorage.getItem('NickName');
      console.log('asd')
    }
    if(localStorage.getItem("jwt") != 'null' ){ //user connected
      const token = localStorage.getItem("jwt");
      if(this.onlineScreen){
        this._hubContact.Start()
        this.onlineScreen = false
      }
      if (token && !this.jwtHelper.isTokenExpired(token)){
        return true;
      }
    }
    return false;
  }
  getContactOffline(){
    this._ContactService.getContact().subscribe((contact)=>
      {
        this.allContact = contact;
      })
  }
  Logout(){
    this.myUser == ""
    window.localStorage.setItem('jwt', "");
    window.localStorage.setItem('NickName', "");
    window.localStorage.setItem('refreshToken', "");
    window.location.reload();  
  }
}
