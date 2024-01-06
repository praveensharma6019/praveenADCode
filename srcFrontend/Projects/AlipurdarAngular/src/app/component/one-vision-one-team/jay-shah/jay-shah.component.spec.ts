import { ComponentFixture, TestBed } from '@angular/core/testing';

import { JayShahComponent } from './jay-shah.component';

describe('JayShahComponent', () => {
  let component: JayShahComponent;
  let fixture: ComponentFixture<JayShahComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [JayShahComponent]
    });
    fixture = TestBed.createComponent(JayShahComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
