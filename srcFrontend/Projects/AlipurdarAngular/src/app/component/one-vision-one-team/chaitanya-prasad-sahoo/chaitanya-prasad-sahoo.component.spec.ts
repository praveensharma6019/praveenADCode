import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChaitanyaPrasadSahooComponent } from './chaitanya-prasad-sahoo.component';

describe('ChaitanyaPrasadSahooComponent', () => {
  let component: ChaitanyaPrasadSahooComponent;
  let fixture: ComponentFixture<ChaitanyaPrasadSahooComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ChaitanyaPrasadSahooComponent]
    });
    fixture = TestBed.createComponent(ChaitanyaPrasadSahooComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
