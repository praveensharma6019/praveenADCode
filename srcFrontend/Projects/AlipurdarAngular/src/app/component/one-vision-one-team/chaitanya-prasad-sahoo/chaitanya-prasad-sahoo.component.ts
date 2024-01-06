import { Component } from '@angular/core';

@Component({
  selector: 'app-chaitanya-prasad-sahoo',
  templateUrl: './chaitanya-prasad-sahoo.component.html',
  styleUrls: ['./chaitanya-prasad-sahoo.component.css']
})
export class ChaitanyaPrasadSahooComponent {
  profile = 'Chaitanya Prasad Sahoo';
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
