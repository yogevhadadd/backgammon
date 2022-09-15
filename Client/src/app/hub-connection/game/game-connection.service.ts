import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { GameStatus } from 'src/app/model/gameStatus';

@Injectable({
  providedIn: 'root'
})
export class GameConnectionService {
  public gameStatus:GameStatus = new GameStatus
  private _hubConnection!: HubConnection;
  public showMove:boolean = true;
  public firstPic:string = "";
  public secondPic:string = "";


  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection() {
    this._hubConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7195/game',{
        withCredentials: false,
        accessTokenFactory: () => localStorage.getItem('jwt')!,
      })
      .build();
  }
  private startConnection(): void {
    this._hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(err => {
        console.log('Error while establishing connection, retrying...');
        setTimeout( () => { this.startConnection(); }, 5000);
      });
  }
  private registerOnServerEvents(): void {
    this._hubConnection.on('PlayerTurn', (data: any,whiteList:any,blackList:any,cube:any,show:any) => {
        this.gameStatus.show = show;
        this.gameStatus.listWhite = whiteList;
        this.gameStatus.listblack= blackList;
        this.gameStatus.cube = cube;
        this.gameStatus.Change(data);
        this.showMove = true;
    });
    this._hubConnection.on('startGame', (data: any,whiteList:any,blackList:any,cube:any,name:String,firstPic:string, secondPic:string) => {
      if(localStorage.getItem('NickName') == name){
        this.gameStatus.myTurn = true;
        this.gameStatus.canRole = true;
        this.gameStatus.myColor = "black";
      }
      else{
        this.gameStatus.myTurn = false;
        this.gameStatus.canRole = false;
        this.gameStatus.myColor = "white";
      }
        this.firstPic = firstPic;
        this.secondPic= secondPic;
        this.gameStatus.listWhite = whiteList;
        this.gameStatus.listblack= blackList;
        this.gameStatus.cube = cube;
        this.gameStatus.Change(data);
        this.showMove = true;
      });
    this._hubConnection.on('FinishTurn',(ate:any)=>{
      this.gameStatus.listWhite = this.gameStatus.listWhite;
      this.gameStatus.listblack = this.gameStatus.listblack;
      this.gameStatus.myTurn = !this.gameStatus.myTurn;
    })
    this._hubConnection.on('role',(cubes:any)=>{
      this.gameStatus.canRole = !this.gameStatus.canRole
      this.gameStatus.cube = cubes;
      this.gameStatus.cube = cubes;
    })
    this._hubConnection.on('CanMove',(cubes:any)=>{
      this.gameStatus.myTurn = !this.gameStatus.myTurn;
      this.gameStatus.cube = cubes;
    })
    this._hubConnection.on('OnMove',(show:any)=>{
      this.gameStatus.show = show;
    })
    this._hubConnection.on('Delete',(data: any,whiteList:any,blackList:any,cube:any)=>{
      this.gameStatus.listWhite = whiteList;
      this.gameStatus.listblack= blackList;
      this.gameStatus.cube = cube;
      this.gameStatus.Change(data);
    })
    this._hubConnection.on('EndGame',(date:any)=>{
      confirm("END GAME!")
    })
  }
  public async sendAction(from: number,to: number) {
    this._hubConnection?.invoke("MyTurn", from,to);
  }
  public async sendActionDelete() {
    this._hubConnection?.invoke("Delete");
  }
  public async RoleCubes() {
    this._hubConnection?.invoke("RoleCubes");
  }
  public async PassTurn() {
    this._hubConnection?.invoke("PassTurn");
  }
  public async OnMove(num:number) {
    this._hubConnection?.invoke("OnMove",num);
  }
}