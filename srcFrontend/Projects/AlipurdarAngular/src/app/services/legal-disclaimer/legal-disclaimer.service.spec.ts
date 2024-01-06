import { TestBed } from '@angular/core/testing';

import { LegalDisclaimerService } from './legal-disclaimer.service';

describe('LegalDisclaimerService', () => {
  let service: LegalDisclaimerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LegalDisclaimerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
