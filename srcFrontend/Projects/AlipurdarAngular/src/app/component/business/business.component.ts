import { Component } from '@angular/core';
import { BusinessService } from 'src/app/services/business/business.service';

@Component({
  selector: 'app-business',
  templateUrl: './business.component.html',
  styleUrls: ['./business.component.css']
})
export class BusinessComponent {
  Business:any;
  constructor(private BusinessPageData:BusinessService)
  {
    BusinessPageData.Business().subscribe((data)=>{
      this.Business=data;
    });
  }
  slideConfig = {"slidesToShow": 1, "slidesToScroll": 1, "dots": true, "infinite": true};
}
