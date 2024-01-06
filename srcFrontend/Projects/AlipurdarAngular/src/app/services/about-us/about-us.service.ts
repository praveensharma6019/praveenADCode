import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AboutUsService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/aboutus";
  // url = '../../../assets/json/about-us.json';
  url = "/api/WesternTransAlipurdarCompoment/AboutUs";
  constructor(private http: HttpClient) { }

  AboutUs() {
    return this.http.get(this.url);
  }
}
