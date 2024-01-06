import { Component } from '@angular/core';
import { InvestorService } from '../../services/investor/investor.service'
@Component({
  selector: 'app-investor',
  templateUrl: './investor.component.html',
  styleUrls: ['./investor.component.css']
})
export class InvestorComponent {
  Investor: any;
  constructor(private InvestorPageData: InvestorService) {
    InvestorPageData.Investor().subscribe((data) => {
      this.Investor = data;
    });
  }
}
