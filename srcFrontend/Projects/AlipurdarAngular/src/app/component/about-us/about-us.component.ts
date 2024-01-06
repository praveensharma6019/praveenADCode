import { Component } from '@angular/core';
import { AboutUsService } from 'src/app/services/about-us/about-us.service';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.css']
})
export class AboutUsComponent {
  AboutUs: any;
  constructor(private AboutUsPageData: AboutUsService) {
    AboutUsPageData.AboutUs().subscribe((data) => {
      this.AboutUs = data;
    });
  }
}
