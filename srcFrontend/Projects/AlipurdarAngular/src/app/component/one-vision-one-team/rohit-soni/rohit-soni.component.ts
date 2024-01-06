import { Component } from '@angular/core';

@Component({
  selector: 'app-rohit-soni',
  templateUrl: './rohit-soni.component.html',
  styleUrls: ['./rohit-soni.component.css']
})
export class RohitSoniComponent {
  profile = 'rohit-soni';
  profiledata: any;
  constructor() {
    OneVisionOneTeamMember(this.profile).then((profiledata: any) => {
      this.profiledata = profiledata;
    });
  }
}

async function OneVisionOneTeamMember(Profile: any): Promise<any> {
  //var queryUrl = "http://westerntranswebapi.dev.local/api/WesternTransAlipurdarCompoment/OneVisionOneTeam?Profile="+Profile;
  var queryUrl = "/api/WesternTransAlipurdarCompoment/OneVisionOneTeam?Profile=" + Profile;

  const queryData = await fetch(queryUrl);
  return await queryData.json() ?? {};
}
