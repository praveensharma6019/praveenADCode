import { Component } from '@angular/core';
import { LegalDisclaimerService } from 'src/app/services/legal-disclaimer/legal-disclaimer.service';

@Component({
  selector: 'app-legal-disclaimer',
  templateUrl: './legal-disclaimer.component.html',
  styleUrls: ['./legal-disclaimer.component.css']
})
export class LegalDisclaimerComponent {
  LegalDisclaimer: any;
  constructor(private LegalDisclaimerPageData: LegalDisclaimerService) {
    LegalDisclaimerPageData.LegalDisclaimer().then((data) => {
      this.LegalDisclaimer = data;
    });
  }
}
