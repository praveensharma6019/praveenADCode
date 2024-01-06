import { Component, OnInit } from '@angular/core';
import { HomepageService } from '../../services/home/homepage.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  HomePage: any;
  collapsed = true;

  constructor(private HomePageData: HomepageService) {
    //   HomePageData.HomePage().subscribe((data) => {
    //     this.HomePage = data;
    //     console.log(data);
    //   });
    // }
    // export class HeaderComponent implements OnInit {
    // users: any;
    // constructor(private userData: UserService) {

    HomePageData.HomePage().then((data: any) => {
      console.log(data);
      this.HomePage = data;
    })
  }

  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  slideConfigBanner = { "slidesToShow": 1, "slidesToScroll": 1, "dots": true, "infinite": true };
  slideConfigOurGroup = { "slidesToShow": 6, "slidesToScroll": 1, "dots": false, "infinite": true, responsive: [{ breakpoint: 1024, settings: { slidesToShow: 6, slidesToScroll: 1, }, }, { breakpoint: 800, settings: { slidesToShow: 4, slidesToScroll: 1, }, }, { breakpoint: 600, settings: { slidesToShow: 2, dots: false, slidesToScroll: 1, }, }, { breakpoint: 300, settings: { slidesToShow: 1, slidesToScroll: 1, }, }] };
  slideConfigCommercial = { "slidesToShow": 3, "slidesToScroll": 1, "dots": false, "infinite": true, responsive: [{ breakpoint: 1024, settings: { slidesToShow: 3, slidesToScroll: 1, }, }, { breakpoint: 600, settings: { slidesToShow: 1, slidesToScroll: 1, }, }] };
}
