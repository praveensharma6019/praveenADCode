import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TermsOfUseService {

  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/TermsOfUse";
  url = "/api/WesternTransAlipurdarCompoment/TermsOfUse";
  // url = '../../../assets/json/terms-of-use.json';

  constructor(private http: HttpClient) { }

  // PrivacyPolicy() {
  //   return this.http.get(this.url);
  // }

  async TermsOfUse(): Promise<any> {

    const data = await fetch(this.url, {

    });
    return await data.json() ?? {};
  }
}
