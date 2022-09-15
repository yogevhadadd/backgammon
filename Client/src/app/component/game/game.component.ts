import { CdkDragDrop} from '@angular/cdk/drag-drop';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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

  ngOnInit(): void {}
  constructor(public gameConnectionService:GameConnectionService) { }

  PassTurn(){
    this.gameConnectionService.PassTurn();
  }
  Undo(){
    this.gameConnectionService.sendActionDelete();
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
      this.gameConnectionService.sendAction(event.previousContainer.data.index, event.container.data.index);
      this.canDrop = false;
    }
  }
  OnMove(event: number){
    if(this.gameConnectionService.showMove){
      this.gameConnectionService.OnMove(event);
      this.gameConnectionService.showMove = false;
    }
  }
  RoleCubes(){
    this.gameConnectionService.RoleCubes();
  }
  NumSequence(num?: number): Array<number> {
    return Array(num);
  }
}
