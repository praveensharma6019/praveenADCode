import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {HttpClientModule} from '@angular/common/http';
import { InvestorComponent } from './component/investor/investor.component';
import { HomeComponent } from './component/home/home.component';
import { NgbModule ,NgbCarouselModule} from '@ng-bootstrap/ng-bootstrap';
import {SlickCarouselModule} from "ngx-slick-carousel";
import { BusinessComponent } from './component/business/business.component';
import { TermsOfUseComponent } from './component/terms-of-use/terms-of-use.component';
import { PrivacyPolicyComponent } from './component/privacy-policy/privacy-policy.component';
import { LegalDisclaimerComponent } from './component/legal-disclaimer/legal-disclaimer.component';
import { AboutUsComponent } from './component/about-us/about-us.component';
import { OneVisionOneTeamComponent } from './component/one-vision-one-team/one-vision-one-team.component';
import { HeaderComponent } from './component/header/header.component';
import { FooterComponent } from './component/footer/footer.component';
import { ChairmansMessageComponent } from './component/chairmans-message/chairmans-message.component';
import { InvestorDownloalsComponent } from './component/investor-downloals/investor-downloals.component';
import { CorporateGovernanceComponent } from './component/corporate-governance/corporate-governance.component';
import { ContactUsComponent } from './component/contact-us/contact-us.component';
import { RohitSoniComponent } from './component/one-vision-one-team/rohit-soni/rohit-soni.component';
import { ChaitanyaPrasadSahooComponent } from './component/one-vision-one-team/chaitanya-prasad-sahoo/chaitanya-prasad-sahoo.component';
import { NiharRajComponent } from './component/one-vision-one-team/nihar-raj/nihar-raj.component';
import { ChitraBhatnagarComponent } from './component/one-vision-one-team/chitra-bhatnagar/chitra-bhatnagar.component';
import { JayShahComponent } from './component/one-vision-one-team/jay-shah/jay-shah.component';
@NgModule({
  declarations: [
    AppComponent,
    InvestorComponent,
    HomeComponent,
    BusinessComponent,
    TermsOfUseComponent,
    PrivacyPolicyComponent,
    LegalDisclaimerComponent,
    AboutUsComponent,
    OneVisionOneTeamComponent,
    HeaderComponent,
    FooterComponent,
    ChairmansMessageComponent,
    InvestorDownloalsComponent,
    CorporateGovernanceComponent,
    ContactUsComponent,
    RohitSoniComponent,
    ChaitanyaPrasadSahooComponent,
    NiharRajComponent,
    ChitraBhatnagarComponent,
    JayShahComponent  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    NgbModule,
    NgbCarouselModule,
    SlickCarouselModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {
 }
