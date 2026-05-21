import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';

import { DashboardService } from './dashboard';

describe('DashboardService', () => {
  let service: DashboardService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        DashboardService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(DashboardService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should get dashboard summary', () => {
    service.getSummary().subscribe(result => {
      expect(result.totalCompanies).toBe(2);
      expect(result.totalInterviews).toBe(5);
    });

    const req = httpMock.expectOne('https://localhost:7185/api/Dashboard/summary');

    expect(req.request.method).toBe('GET');

    req.flush({
      totalCompanies: 2,
      totalRecruiters: 1,
      totalInterviews: 5,
      appliedCount: 2,
      technicalInterviewCount: 1,
      offerCount: 1,
      rejectedCount: 1,
      averageExpectedSalary: 4500
    });
  });
});
