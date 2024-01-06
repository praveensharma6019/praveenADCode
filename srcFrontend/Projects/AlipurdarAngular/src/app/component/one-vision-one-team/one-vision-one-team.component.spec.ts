import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OneVisionOneTeamComponent } from './one-vision-one-team.component';

describe('OneVisionOneTeamComponent', () => {
  let component: OneVisionOneTeamComponent;
  let fixture: ComponentFixture<OneVisionOneTeamComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [OneVisionOneTeamComponent]
    });
    fixture = TestBed.createComponent(OneVisionOneTeamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
