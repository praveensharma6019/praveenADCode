import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class CorporateGovernanceService {

  // apiurl = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/CorporateGovernance";
  apiurl = "/api/WesternTransAlipurdarCompoment/CorporateGovernance";
  constructor(private http: HttpClient) { }

  async CorporateGovernance(): Promise<any> {
    const servicedata = await fetch(this.apiurl);
    return await servicedata.json() ?? {};
  }
}
