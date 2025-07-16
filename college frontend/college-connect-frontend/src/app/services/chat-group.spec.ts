import { TestBed } from '@angular/core/testing';

import { ChatGroup } from './chat-group';

describe('ChatGroup', () => {
  let service: ChatGroup;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatGroup);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
