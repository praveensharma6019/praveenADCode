import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NiharRajComponent } from './nihar-raj.component';

describe('NiharRajComponent', () => {
  let component: NiharRajComponent;
  let fixture: ComponentFixture<NiharRajComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [NiharRajComponent]
    });
    fixture = TestBed.createComponent(NiharRajComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
