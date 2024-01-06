import { TestBed } from '@angular/core/testing';

import { CorporateGovernanceService } from './corporate-governance.service';

describe('CorporateGovernanceService', () => {
  let service: CorporateGovernanceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CorporateGovernanceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
