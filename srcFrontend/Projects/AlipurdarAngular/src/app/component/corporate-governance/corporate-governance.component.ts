import { Component } from '@angular/core';
import { CorporateGovernanceService } from 'src/app/services/corporate-governance/corporate-governance.service';

@Component({
  selector: 'app-corporate-governance',
  templateUrl: './corporate-governance.component.html',
  styleUrls: ['./corporate-governance.component.css']
})
export class CorporateGovernanceComponent {
  CorporateGovernance: any;
  Profilesectiondata: any;
  ManagerialInformation: any;
  CommitteeComposition: any;
  description: string = "";
  activeIndex: number = 0;
  description2: string = "";
  activeIndex2: number = 0;
  description3: string = "";
  activeIndex3: number = 0;
  constructor(private CorporateGovernancePageData: CorporateGovernanceService) {



  }
  ngOnInit() {
    this.CorporateGovernancePageData.CorporateGovernance().then((servicedata: any) => {
      this.CorporateGovernance = servicedata;
      this.Profilesectiondata = servicedata.CorporateGoveranceProfiles.ProfileSections[0];
      this.description = this.Profilesectiondata.ProfileSectionItems[0].HTMLText;

      this.ManagerialInformation = servicedata.CorporateGoveranceProfiles.ProfileSections[1];
      this.description2 = this.ManagerialInformation.ProfileSectionItems[1].HTMLText;

      this.CommitteeComposition = servicedata.CorporateGoveranceProfiles.ProfileSections[2];
      this.description3 = this.CommitteeComposition.ProfileSectionItems[0].HTMLText;

    });

  }

  changeDescription(data: any, index: number) {

    this.description = data.HTMLText;
    this.activeIndex = index;

  }
  changeDescription2(data: any, index: number) {

    this.description2 = data.HTMLText;
    this.activeIndex2 = index;

  }
  changeDescription3(data: any, index: number) {

    this.description3 = data.HTMLText;
    this.activeIndex3 = index;

  }

}
