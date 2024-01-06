import { Component } from '@angular/core';

@Component({
  selector: 'app-chitra-bhatnagar',
  templateUrl: './chitra-bhatnagar.component.html',
  styleUrls: ['./chitra-bhatnagar.component.css']
})
export class ChitraBhatnagarComponent {
  profile = 'Chitra Bhatnagar';
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
