import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PrivacyPolicyService {
  //  url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/PrivacyPolicy";
  url = "/api/WesternTransAlipurdarCompoment/PrivacyPolicy";
  // url = '../../../assets/json/privacy-policy.json';

  constructor(private http: HttpClient) { }

  // PrivacyPolicy() {
  //   return this.http.get(this.url);
  // }

  async PrivacyPolicy(): Promise<any> {

    const data = await fetch(this.url, {

    });
    return await data.json() ?? {};
  }
}
