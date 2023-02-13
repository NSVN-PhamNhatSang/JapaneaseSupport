import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InforDetailComponent } from './infor-detail.component';

describe('InforDetailComponent', () => {
  let component: InforDetailComponent;
  let fixture: ComponentFixture<InforDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InforDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InforDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
