import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RohitSoniComponent } from './rohit-soni.component';

describe('RohitSoniComponent', () => {
  let component: RohitSoniComponent;
  let fixture: ComponentFixture<RohitSoniComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RohitSoniComponent]
    });
    fixture = TestBed.createComponent(RohitSoniComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
