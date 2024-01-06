import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InvestorComponent } from './component/investor/investor.component';
import { HomeComponent } from './component/home/home.component';
import { BusinessComponent } from './component/business/business.component';
import { PrivacyPolicyComponent } from './component/privacy-policy/privacy-policy.component';
import { LegalDisclaimerComponent } from './component/legal-disclaimer/legal-disclaimer.component';
import { TermsOfUseComponent } from './component/terms-of-use/terms-of-use.component';
import { AboutUsComponent } from './component/about-us/about-us.component';
import { OneVisionOneTeamComponent } from './component/one-vision-one-team/one-vision-one-team.component';
import { ChairmansMessageComponent } from './component/chairmans-message/chairmans-message.component';
import { InvestorDownloalsComponent } from './component/investor-downloals/investor-downloals.component';
import { CorporateGovernanceComponent } from './component/corporate-governance/corporate-governance.component';
import { ContactUsComponent } from './component/contact-us/contact-us.component';
import { RohitSoniComponent } from './component/one-vision-one-team/rohit-soni/rohit-soni.component';
import { NiharRajComponent } from './component/one-vision-one-team/nihar-raj/nihar-raj.component';
import { JayShahComponent } from './component/one-vision-one-team/jay-shah/jay-shah.component';
import { ChitraBhatnagarComponent } from './component/one-vision-one-team/chitra-bhatnagar/chitra-bhatnagar.component';
import { ChaitanyaPrasadSahooComponent } from './component/one-vision-one-team/chaitanya-prasad-sahoo/chaitanya-prasad-sahoo.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'investor', component: InvestorComponent },
  { path: 'investorDownload', component: InvestorDownloalsComponent },
  { path: 'business', component: BusinessComponent },
  { path: 'privacy-policy', component: PrivacyPolicyComponent },
  { path: 'legal-disclaimer', component: LegalDisclaimerComponent },
  { path: 'term-of-use', component: TermsOfUseComponent },
  { path: 'about-us', component: AboutUsComponent },
  { path: 'about-us/One-Vision-One-Team', component: OneVisionOneTeamComponent },
  { path: 'about-us/chairmans-message', component: ChairmansMessageComponent },
  { path: 'corporate-governance', component: CorporateGovernanceComponent },
  { path: 'contact-us', component: ContactUsComponent },
  { path: 'about-us/One-Vision-One-Team/rohit-soni', component: RohitSoniComponent },
  { path: 'about-us/One-Vision-One-Team/nihar-raj', component: NiharRajComponent },
  { path: 'about-us/One-Vision-One-Team/jay-shah', component: JayShahComponent },
  { path: 'about-us/One-Vision-One-Team/chitra-bhatnagar', component: ChitraBhatnagarComponent },
  { path: 'about-us/One-Vision-One-Team/chaitanya-prasad-sahoo', component: ChaitanyaPrasadSahooComponent },


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
