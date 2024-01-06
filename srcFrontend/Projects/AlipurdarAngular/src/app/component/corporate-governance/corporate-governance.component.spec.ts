import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CorporateGovernanceComponent } from './corporate-governance.component';

describe('CorporateGovernanceComponent', () => {
  let component: CorporateGovernanceComponent;
  let fixture: ComponentFixture<CorporateGovernanceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CorporateGovernanceComponent]
    });
    fixture = TestBed.createComponent(CorporateGovernanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
