import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InforCourseComponent } from './infor-course.component';

describe('InforCourseComponent', () => {
  let component: InforCourseComponent;
  let fixture: ComponentFixture<InforCourseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InforCourseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InforCourseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
