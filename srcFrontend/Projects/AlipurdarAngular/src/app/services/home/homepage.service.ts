import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HomepageService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/home";
  url = "/api/WesternTransAlipurdarCompoment/Home";
  //  url = '../../../assets/json/home.json';

  constructor(private http: HttpClient) { }

  async HomePage(): Promise<any> {
    const data = await fetch(this.url, {

    });
    return await data.json() ?? {};
  }
}
