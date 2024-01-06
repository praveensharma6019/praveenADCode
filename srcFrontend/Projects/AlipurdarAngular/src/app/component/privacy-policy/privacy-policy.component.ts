import { Component } from '@angular/core';
import { PrivacyPolicyService } from 'src/app/services/privacy-policy/privacy-policy.service';

@Component({
  selector: 'app-privacy-policy',
  templateUrl: './privacy-policy.component.html',
  styleUrls: ['./privacy-policy.component.css']
})
export class PrivacyPolicyComponent {
  privacyPolicy: any;
  constructor(private PrivacyPolicyPageData: PrivacyPolicyService) {
    PrivacyPolicyPageData.PrivacyPolicy().then((data) => {
      this.privacyPolicy = data;
    });
  }
}
