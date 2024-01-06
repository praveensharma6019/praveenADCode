import { Component, OnInit } from '@angular/core';
import { HomepageService } from 'src/app/services/home/homepage.service';


@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {

  Header: any;
  collapsed = true;
  constructor(private HeaderData: HomepageService) {
    //HeaderData.HomePage().subscribe((data: any) => {
    HeaderData.HomePage().then((data: any) => {

      this.Header = data;
      console.log(this.Header);
    });

  }

  menuVariable: boolean = false;
  menu_icon_close: boolean = false;
  openMenu() {
    this.menuVariable = !this.menuVariable;
    this.menu_icon_close = !this.menu_icon_close;

  }
}