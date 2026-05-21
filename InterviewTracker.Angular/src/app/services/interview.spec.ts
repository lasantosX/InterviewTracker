import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';

import { InterviewService } from './interview';

describe('InterviewService', () => {
  let service: InterviewService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InterviewService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(InterviewService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should get interviews with pagination and status', () => {
    service.getInterviews(1, 10, 'Applied').subscribe((result) => {
      expect(result.totalCount).toBe(1);
      expect(result.items.length).toBe(1);
    });

    const req = httpMock.expectOne(
      'https://localhost:7185/api/Interviews?pageNumber=1&pageSize=10&status=Applied',
    );

    expect(req.request.method).toBe('GET');

    req.flush({
      items: [
        {
          id: 1,
          roleTitle: 'Senior .NET Developer',
          status: 'Applied',
          companyId: 1,
          companyName: 'TechNova',
          createdAt: '2026-05-20T15:00:00',
        },
      ],
      pageNumber: 1,
      pageSize: 10,
      totalCount: 1,
      totalPages: 1,
    });
  });

  it('should create interview', () => {
    const request = {
      roleTitle: 'Senior .NET Developer',
      status: 'Applied',
      companyId: 1,
    };

    service.createInterview(request).subscribe((result) => {
      expect(result.roleTitle).toBe('Senior .NET Developer');
    });

    const req = httpMock.expectOne('https://localhost:7185/api/Interviews');

    expect(req.request.method).toBe('POST');

    req.flush({
      id: 1,
      roleTitle: 'Senior .NET Developer',
      status: 'Applied',
      companyId: 1,
      createdAt: '2026-05-20T15:00:00',
    });
  });

  it('should delete interview', () => {
    service.deleteInterview(1).subscribe((result) => {
      expect(result).toBeNull();
    });

    const req = httpMock.expectOne('https://localhost:7185/api/Interviews/1');

    expect(req.request.method).toBe('DELETE');

    req.flush(null);
  });
});
