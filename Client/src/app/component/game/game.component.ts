import { CdkDragDrop} from '@angular/cdk/drag-drop';
import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { GameConnectionService } from 'src/app/hub-connection/game/game-connection.service';
@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})

export class GameComponent implements OnInit {
  @ViewChild('dropListContainer') dropListContainer?: ElementRef;
  dropListReceiverElement?: HTMLElement;
  canDrop: Boolean = false;
  @Input() gameUser = "";

  ngOnInit(): void {}
  constructor(public gameConnectionService:GameConnectionService) {
   }

  PassTurn(){
    if(this.gameConnectionService.gameStatus.myTurn){
      this.gameConnectionService.PassTurn();
    }
    
  }
  Undo(){
      this.gameConnectionService.sendActionDelete(this.gameUser);
  }
  OnDrop(event: CdkDragDrop<any>){
    this.gameConnectionService.showMove = true;
    for(let i = 0;i < 25;i++){
      if(this.gameConnectionService.gameStatus.show[i]){
        this.canDrop = true;
      }
      this.gameConnectionService.gameStatus.show[i] = false;
    }
    if(this.canDrop){
      this.gameConnectionService.MyTurn(event.previousContainer.data.index, event.container.data.index, this.gameUser);
      this.canDrop = false;
      }
    
  }
  OnMove(event: number){
    if(this.gameConnectionService.showMove){
      this.gameConnectionService.OnMove(event,this.gameUser);
      this.gameConnectionService.showMove = false;
    }
  }
  RoleCubes(){
    this.gameConnectionService.RoleCubes(this.gameUser);
  }
  NumSequence(num?: number): Array<number> {
    return Array(num);
  }
}
