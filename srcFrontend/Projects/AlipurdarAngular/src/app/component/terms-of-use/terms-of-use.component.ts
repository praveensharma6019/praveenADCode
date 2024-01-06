import { Component } from '@angular/core';
import { TermsOfUseService } from 'src/app/services/terms-of-use/terms-of-use.service';

@Component({
  selector: 'app-terms-of-use',
  templateUrl: './terms-of-use.component.html',
  styleUrls: ['./terms-of-use.component.css']
})
export class TermsOfUseComponent {
  TermsOfUse: any;
  constructor(private TermsOfUsePageData: TermsOfUseService) {
    TermsOfUsePageData.TermsOfUse().then((data) => {
      this.TermsOfUse = data;
    });
  }
}
