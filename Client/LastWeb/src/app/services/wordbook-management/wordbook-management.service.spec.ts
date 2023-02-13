import { TestBed } from '@angular/core/testing';

import { WordbookManagementService } from './wordbook-management.service';

describe('WordbookManagementService', () => {
  let service: WordbookManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(WordbookManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
