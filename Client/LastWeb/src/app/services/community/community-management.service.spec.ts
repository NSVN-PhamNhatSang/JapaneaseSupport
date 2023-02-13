import { TestBed } from '@angular/core/testing';

import { CommunityManagementService } from './community-management.service';

describe('CommunityManagementService', () => {
  let service: CommunityManagementService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommunityManagementService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
