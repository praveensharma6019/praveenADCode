import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LegalDisclaimerService {

  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/LegalDisclaimer";
  url = "/api/WesternTransAlipurdarCompoment/LegalDisclaimer";
  // url = '../../../assets/json/legal-disclaimer.json';

  static LegalDisclaimer: any;
  constructor(private http: HttpClient) { }

  async LegalDisclaimer(): Promise<any> {

    const data = await fetch(this.url, {

    });
    return await data.json() ?? {};
  }

  // LegalDisclaimer() {
  //   return this.http.get(this.url);
  // }
}
