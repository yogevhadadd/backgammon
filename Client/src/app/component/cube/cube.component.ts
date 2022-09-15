import { Component, Input, OnInit} from '@angular/core';
import { interval } from 'rxjs';

@Component({
  selector: 'app-cube',
  templateUrl: './cube.component.html',
  styleUrls: ['./cube.component.css']
})
export class CubeComponent implements OnInit {
  @Input() currentClass = "";
  @Input() color = "";
  @Input() active = true;
  num:number = 1;
  constructor() { 
  }

  ngOnInit(): void {
    if(this.active){
      this.num = Math.floor((Math.random()*6)) + 1
      interval(1000).subscribe(()=>{
        this.currentClass = "show-login-" + this.num.toString()
        this.num += 1;
        if(this.num == 8){
          this.num = 1
        }
      });
    }
  }

}
