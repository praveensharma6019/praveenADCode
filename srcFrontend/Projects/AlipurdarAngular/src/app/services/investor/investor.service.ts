import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
@Injectable({
  providedIn: 'root'
})
export class InvestorService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/Investor";
  url = "/api/WesternTransAlipurdarCompoment/Investor";
  // url = '../../../assets/json/investor.json';
  constructor(private http: HttpClient) { }

  Investor() {
    return this.http.get(this.url);
  }
}
