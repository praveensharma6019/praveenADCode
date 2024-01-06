import { Component } from '@angular/core';

@Component({
  selector: 'app-nihar-raj',
  templateUrl: './nihar-raj.component.html',
  styleUrls: ['./nihar-raj.component.css']
})
export class NiharRajComponent {
  profile = 'Nihar Raj';
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