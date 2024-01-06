import { Component } from '@angular/core';
import { OneVisionOneTeamService } from 'src/app/services/OneVisionOneTeam/one-vision-one-team.service';

@Component({
  selector: 'app-one-vision-one-team',
  templateUrl: './one-vision-one-team.component.html',
  styleUrls: ['./one-vision-one-team.component.css']
})
export class OneVisionOneTeamComponent {
  OneVisionOneTeam:any;
  constructor(private OneVisionOneTeamPageData:OneVisionOneTeamService)
  {
    OneVisionOneTeamPageData.OneVisionOneTeam().subscribe((data)=>{
      this.OneVisionOneTeam=data;
    });
  }
}
