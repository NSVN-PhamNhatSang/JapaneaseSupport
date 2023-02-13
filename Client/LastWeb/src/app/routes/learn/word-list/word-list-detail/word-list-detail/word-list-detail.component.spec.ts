import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WordListDetailComponent } from './word-list-detail.component';

describe('WordListDetailComponent', () => {
  let component: WordListDetailComponent;
  let fixture: ComponentFixture<WordListDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WordListDetailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WordListDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
