import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatedWorkbookComponent } from './created-workbook.component';

describe('CreatedWorkbookComponent', () => {
  let component: CreatedWorkbookComponent;
  let fixture: ComponentFixture<CreatedWorkbookComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatedWorkbookComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatedWorkbookComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
