import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ContactComponent } from './component/contact/contact.component';
import { LoginComponent } from './component/login/login.component';
import { ChatComponent } from './component/chat/chat.component';
import { JwtModule } from '@auth0/angular-jwt';
import { GameComponent } from './component/game/game.component';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { AuthGuard } from './guards/auth.guard';
import { CubeComponent } from './component/cube/cube.component';
export function tokenGetter() { 
  return localStorage.getItem("jwt"); 
}
@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    ContactComponent,
    ChatComponent,
    GameComponent,
    CubeComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule, 
    ReactiveFormsModule,
    FormsModule,
    DragDropModule,
    HttpClientModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: tokenGetter,
        allowedDomains: ["localhost:7088","localhost:7048","localhost:7195","localhost:44347"],
        disallowedRoutes: []
      }
    })
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }




// import { Injectable } from '@angular/core';
// import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
// import { GameStatus } from 'src/app/model/gameStatus';
// import { PicesOfBoard } from 'src/app/model/Location';

// @Injectable({
//   providedIn: 'root'
// })
// export class GameConnectionService {
//   public gameStatus:GameStatus = new GameStatus
//   private _hubConnection!: HubConnection;
//   public myColor:any = "";
//   public showMove:boolean = true;
//   constructor() {
//     this.myColor = localStorage.getItem('NickNamePlayerColor')
//     if(this.myColor == "black"){
//       this.gameStatus.myTurn = true;
//       this.gameStatus.canRole = true;
//     }
//     this.createConnection();
//     this.registerOnServerEvents();
//     this.startConnection();
//   }

//   private createConnection() {
//     this._hubConnection = new HubConnectionBuilder()
//     .withUrl('https://localhost:7195/game',{
//         withCredentials: false,
//         accessTokenFactory: () => localStorage.getItem('jwt')!,
//       })
//       .build();
//   }
//   private startConnection(): void {
//     this._hubConnection
//       .start()
//       .then(() => {
//         console.log('Hub connection started');
//       })
//       .catch(err => {
//         console.log('Error while establishing connection, retrying...');
//         setTimeout( () => { this.startConnection(); }, 5000);
//       });
//   }
//   private registerOnServerEvents(): void {
//     this._hubConnection.on('PlayerTurn', (data: PicesOfBoard[],whiteList:any,blackList:any,cube:any,show:any) => {
//         this.gameStatus.listWhite = whiteList;
//         this.gameStatus.listblack= blackList;
//         this.gameStatus.cube = cube;
//         this.gameStatus.Change(data);
//         this.showMove = true;
//         this.gameStatus.show = show;
//     });
//     this._hubConnection.on('startGame', (data: any,whiteList:any,blackList:any,cube:any,name:any) => {
//       this.gameStatus.listWhite = whiteList;
//       this.gameStatus.listblack= blackList;
//       this.gameStatus.cube = cube;
//       this.gameStatus.Change(data);
//       this.showMove = true;
//   });
//     this._hubConnection.on('EndGame',(date:any)=>{
//       confirm("END GAME!")
//     })
//     this._hubConnection.on('FinishTurn',(ate:any)=>{
//       this.gameStatus.listWhite = this.gameStatus.listWhite;
//       this.gameStatus.listblack = this.gameStatus.listblack;
//       this.gameStatus.myTurn = !this.gameStatus.myTurn;
//     })
//     this._hubConnection.on('role',(cubes:any)=>{
//       this.gameStatus.canRole = !this.gameStatus.canRole
//       this.gameStatus.cube = cubes;
//       this.gameStatus.cube = cubes;
//     })
//     this._hubConnection.on('CanMove',(cubes:any)=>{
//       this.gameStatus.myTurn = !this.gameStatus.myTurn;
//       this.gameStatus.cube = cubes;
//     })
//     this._hubConnection.on('OnMove',(show:any)=>{
//       this.gameStatus.show = show;
//     })
//     this._hubConnection.on('Delete',(data: any,whiteList:any,blackList:any,cube:any)=>{
//       this.gameStatus.listWhite = whiteList;
//       this.gameStatus.listblack= blackList;
//       this.gameStatus.cube = cube;
//       this.gameStatus.Change(data);
//     })
//   }
//   public async sendAction(from: number,to: number) {
//     this._hubConnection?.invoke("MyTurn", from,to);
//   }
//   public async sendActionDelete() {
//     this._hubConnection?.invoke("Delete");
//   }
//   public async RoleCubes() {
//     this._hubConnection?.invoke("RoleCubes");
//   }
//   public async PassTurn() {
//     this._hubConnection?.invoke("PassTurn");
//   }
//   public async OnMove(num:number) {
//     this._hubConnection?.invoke("OnMove",num);
//   }
//   public async GetStart() {
//     await this._hubConnection?.invoke("StartNewGame");
//   }
// }