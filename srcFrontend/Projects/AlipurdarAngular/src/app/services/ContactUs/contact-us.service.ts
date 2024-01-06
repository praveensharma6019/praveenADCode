import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ContactUsService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/ContactUs";
  url = "/api/WesternTransAlipurdarCompoment/ContactUs";

  constructor(private http: HttpClient) { }
  async ContactUs(): Promise<any> {

    const data = await fetch(this.url, {

    });
    return await data.json() ?? {};
  }
}
