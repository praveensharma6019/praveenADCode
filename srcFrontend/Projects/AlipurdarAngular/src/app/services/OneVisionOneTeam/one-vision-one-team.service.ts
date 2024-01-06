import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class OneVisionOneTeamService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/OneVisionOneTeam";
  url = "/api/WesternTransAlipurdarCompoment/OneVisionOneTeam";
  // url = "../../../assets/json/legal-disclaimer.json";
  constructor(private http: HttpClient) { }

  OneVisionOneTeam() {
    return this.http.get(this.url);
  }
}
