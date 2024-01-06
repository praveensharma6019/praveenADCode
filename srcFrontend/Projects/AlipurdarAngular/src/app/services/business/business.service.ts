import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BusinessService {

  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/Business";
  url = "/api/WesternTransAlipurdarCompoment/Business";
  //url = '../../../assets/json/business.json';
  constructor(private http: HttpClient) { }

  Business() {
    return this.http.get(this.url);
  }
}
