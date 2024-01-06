import { TestBed } from '@angular/core/testing';

import { ChairmansMessageService } from './chairmans-message.service';

describe('ChairmansMessageService', () => {
  let service: ChairmansMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChairmansMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
