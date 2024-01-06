import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvestorDownloalsComponent } from './investor-downloals.component';

describe('InvestorDownloalsComponent', () => {
  let component: InvestorDownloalsComponent;
  let fixture: ComponentFixture<InvestorDownloalsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InvestorDownloalsComponent]
    });
    fixture = TestBed.createComponent(InvestorDownloalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
