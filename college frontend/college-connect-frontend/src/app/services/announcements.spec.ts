import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { AnnouncementsService, Announcement } from './announcements';

describe('AnnouncementsService', () => {
  let service: AnnouncementsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AnnouncementsService]
    });

    service = TestBed.inject(AnnouncementsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no outstanding HTTP requests
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch announcements', () => {
    const mockData: Announcement[] = [
      {
        id: 1,
        title: 'Test',
        message: 'Test Body',
        postedById: 42,
        postedByName: 'Tester',
        category: 'general',
        postedOn: '2025-06-11T00:00:00.000Z'
      }
    ];

    service.getAll().subscribe(data => {
      expect(data.length).toBe(1);
      expect(data[0].title).toBe('Test');
      expect(data[0].postedByName).toBe('Tester');
      expect(data[0].category).toBe('general');
    });

    const req = httpMock.expectOne('https://localhost:7057/api/announcement');
    expect(req.request.method).toBe('GET');
    req.flush(mockData);
  });
});
