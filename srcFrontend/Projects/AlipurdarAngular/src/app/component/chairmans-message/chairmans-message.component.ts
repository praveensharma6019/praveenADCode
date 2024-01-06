import { Component } from '@angular/core';
import { ChairmansMessageService } from 'src/app/services/chairmans-message/chairmans-message.service';

@Component({
  selector: 'app-chairmans-message',
  templateUrl: './chairmans-message.component.html',
  styleUrls: ['./chairmans-message.component.css']
})
export class ChairmansMessageComponent {
  ChairmansMessage:any;
  constructor(private ChairmansMessagePageData:ChairmansMessageService)
  {
    ChairmansMessagePageData.ChairmansMessage().subscribe((data)=>{
      this.ChairmansMessage=data;
    });
  }
}
