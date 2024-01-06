import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChitraBhatnagarComponent } from './chitra-bhatnagar.component';

describe('ChitraBhatnagarComponent', () => {
  let component: ChitraBhatnagarComponent;
  let fixture: ComponentFixture<ChitraBhatnagarComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChitraBhatnagarComponent]
    });
    fixture = TestBed.createComponent(ChitraBhatnagarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
