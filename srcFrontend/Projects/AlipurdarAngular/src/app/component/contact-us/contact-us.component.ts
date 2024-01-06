import { Component } from '@angular/core';
import { ContactUsService } from 'src/app/services/ContactUs/contact-us.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.css']
})

export class ContactUsComponent {

  ContactUs: any;
  constructor(private ContactUsPageData: ContactUsService) {

    ContactUsPageData.ContactUs().then((data: any) => {
      debugger;
      this.ContactUs = data;
    });
  }

}
