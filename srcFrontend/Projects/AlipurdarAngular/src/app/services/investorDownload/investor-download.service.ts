import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
@Injectable({
  providedIn: 'root'
})
export class InvestorDownloadService {

  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/investordownload";
  url = "/api/WesternTransAlipurdarCompoment/InvestorDownload";
  // url = '../../../assets/json/investorDownload.json';
  constructor(private http: HttpClient) { }

  InvesterDownload() {
    return this.http.get(this.url);
  }
}
