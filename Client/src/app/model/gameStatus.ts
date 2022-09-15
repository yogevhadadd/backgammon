import { PicesOfBoard } from "./Location";

export class GameStatus
{
    myColor:any = "";
    myTurn:boolean = false;
    canRole:boolean = false;
    board:PicesOfBoard[] = []
    picesOfBoard?:PicesOfBoard;
    listWhite:boolean[] = [];
    listblack:boolean[] = [];
    cube:number[] = [];
    show:boolean[] = [];

    GetOneLocation(num:number):PicesOfBoard{
        return this.board[num];
    }
    GetLocation(): PicesOfBoard[]{
        return this.board;
    }
    Change(Board:PicesOfBoard[]){
        this.board = Board
    }

}


