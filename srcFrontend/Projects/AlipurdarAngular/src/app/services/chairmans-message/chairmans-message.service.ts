import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ChairmansMessageService {
  // url = "http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/ChairmanMesage";
  url = "/api/WesternTransAlipurdarCompoment/ChairmanMesage";
  //  url = '../../../assets/json/chairmans-message.json';
  constructor(private http: HttpClient) { }

  ChairmansMessage() {
    return this.http.get(this.url);
  }
}
