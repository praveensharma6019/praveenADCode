import { TestBed } from '@angular/core/testing';

import { OneVisionOneTeamService } from './one-vision-one-team.service';

describe('OneVisionOneTeamService', () => {
  let service: OneVisionOneTeamService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OneVisionOneTeamService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
