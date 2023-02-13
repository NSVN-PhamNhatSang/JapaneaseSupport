import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyWordbooksComponent } from './my-wordbooks.component';

describe('MyWordbooksComponent', () => {
  let component: MyWordbooksComponent;
  let fixture: ComponentFixture<MyWordbooksComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyWordbooksComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyWordbooksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
