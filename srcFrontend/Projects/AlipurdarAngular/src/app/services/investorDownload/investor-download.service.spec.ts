import { TestBed } from '@angular/core/testing';

import { InvestorDownloadService } from './investor-download.service';

describe('InvestorDownloadService', () => {
  let service: InvestorDownloadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InvestorDownloadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
