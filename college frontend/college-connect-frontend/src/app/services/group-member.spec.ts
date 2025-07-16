import { TestBed } from '@angular/core/testing';

import { GroupMember } from './group-member';

describe('GroupMember', () => {
  let service: GroupMember;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GroupMember);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
