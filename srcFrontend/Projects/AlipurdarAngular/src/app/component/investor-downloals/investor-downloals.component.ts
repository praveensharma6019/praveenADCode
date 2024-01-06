import { Component } from '@angular/core';
import { InvestorDownloadService } from 'src/app/services/investorDownload/investor-download.service';

@Component({
  selector: 'app-investor-downloals',
  templateUrl: './investor-downloals.component.html',
  styleUrls: ['./investor-downloals.component.css']
})
export class InvestorDownloalsComponent {
  InvesterDownload:any;
  constructor(private InvestorPageData:InvestorDownloadService)
  {
    InvestorPageData.InvesterDownload().subscribe((data)=>{
      this.InvesterDownload=data;
    });
  }
}
