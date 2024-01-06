import { Component, ElementRef } from '@angular/core';
import { HomepageService } from 'src/app/services/home/homepage.service';
import { LegalDisclaimerService } from 'src/app/services/legal-disclaimer/legal-disclaimer.service';
import { PrivacyPolicyService } from 'src/app/services/privacy-policy/privacy-policy.service';
import { TermsOfUseService } from 'src/app/services/terms-of-use/terms-of-use.service';
import { CorporateGovernanceService } from 'src/app/services/corporate-governance/corporate-governance.service';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Component({
  selector: 'app-footer',
  templateUrl: './footer.component.html',
  styleUrls: ['./footer.component.css']
})
export class FooterComponent {
  Footer: any;
  collapsed = true;
  legalDisclaimer: any;
  privacyPolicyData: any;
  termsOfUse: any;
  corporateGovernance: any;
  SocialLinks: any;
  FooterData: any;
  socialMedia: any;
  postId: any;
  footerdata: any;
  showThankPopUp: Boolean = false;


  constructor(
    private footerData: HomepageService,
    private legalDisclamer: LegalDisclaimerService,
    private privacyPolicyService: PrivacyPolicyService,
    private termsOfUseService: TermsOfUseService,
    private corporateGovernanceService: CorporateGovernanceService,
    private http: HttpClient,
    private eleRef: ElementRef
  ) {
    footerData.HomePage().then((data) => {
      this.footerdata = data.FooterData;
      this.Footer = data;
    });

    legalDisclamer.LegalDisclaimer().then((data: any) => {
      this.legalDisclaimer = data;
    });

    privacyPolicyService.PrivacyPolicy().then((data: any) => {
      this.privacyPolicyData = data;
    });

    termsOfUseService.TermsOfUse().then((data: any) => {
      this.termsOfUse = data;
    });

    corporateGovernanceService.CorporateGovernance().then((data: any) => {
      this.corporateGovernance = data;
    });
  }

  ContactForm(): any {

    let Name = this.eleRef.nativeElement.getElementsByClassName('txtname')[0].firstElementChild.value.trim();

    let Email = this.eleRef.nativeElement.getElementsByClassName('txtemail')[0].firstElementChild.value.trim();

    let Message = this.eleRef.nativeElement.getElementsByClassName('txtcomment')[0].firstElementChild.value.trim();

    let Inquiry = this.eleRef.nativeElement.getElementsByClassName('enquiry')[0].value.trim();

    let Captcha = this.eleRef.nativeElement.getElementsByClassName('captchaResp')[0].value;


    if (Name.length == 0) {
      alert("* Please enter name!");
      return false;
    }

    if (!/^[a-zA-Z ]+$/.test(Name)) {
      alert("* Please enter valid name!");
      return false;
    }

    if (Email.length == 0) {
      alert("* Please enter email!");
      return false;
    }

    if (!/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(Email)) {
      alert("* Please enter valid email!");
      return false;
    }

    if (Message.length == 0) {
      alert("* Please enter your message");
      return false;
    }

    if (!/^[\w\s.,@:!?-]+$/.test(Message)) {
      alert("* Please enter valid message");
      return false;
    }

    if (Inquiry.length == 0) {
      alert("Please select valid enquiry type!");
      return false;
    }

    if (Captcha.length == 0) {
      alert("Captcha is requried!");
      return false;
    }

    const url = '/api/WesternTransAlipurdarCompoment/ContactForm';
    //const url = 'http://westerntransalipurdar.dev.local/api/WesternTransAlipurdarCompoment/ContactForm';

    const httpOptions = {
      headers: new HttpHeaders()
        .set('g-recaptcha-response', Captcha)
        .set('AntiforgeryToken', '0987u645t')
        .set('Access-Control-Allow-Origin', '*')
        .set('Referrer-Policy', 'no-referrer')
        .set('Access-Control-Allow-Methods', 'POST, GET, OPTIONS')
        .set('Content-Type', 'application/json')
        .set('Cookie', 'ASP.NET_SessionId=emwqaqr25ohfjx3xhjndo03g; SC_ANALYTICS_GLOBAL_COOKIE=390b12b6c90a4066a176baf5646a81d9|False'),
    }
    const postData = {
      Name: Name,
      Email: Email,
      Message: Message,
      Inquiry: Inquiry
    };

    this.http.post<any>(url, postData, httpOptions).subscribe((data: any) => {
      this.eleRef.nativeElement.getElementsByClassName('txtname')[0].firstElementChild.value = "";
      this.eleRef.nativeElement.getElementsByClassName('txtemail')[0].firstElementChild.value = "";
      this.eleRef.nativeElement.getElementsByClassName('txtcomment')[0].firstElementChild.value = "";
      this.eleRef.nativeElement.getElementsByClassName('captchaResp')[0].value = "";
      this.eleRef.nativeElement.getElementsByClassName('enquiry')[0].value = "General Inquiry";
      console.log(data);
      if (data.Status) {
        this.showThankPopUp = true;
      }
      else {
        this.showThankPopUp = false;
        alert(data.Result);
      }
    });
  }
  closeModal() {
    this.showThankPopUp = false;
  }
}

