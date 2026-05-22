import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';

import { CompanyService, Company, PagedResult } from './company';

describe('CompanyService', () => {
  let service: CompanyService;
  let httpMock: HttpTestingController;

  const mockApiUrl = 'https://localhost:7185/api/Companies';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CompanyService, provideHttpClient(), provideHttpClientTesting()],
    });

    service = TestBed.inject(CompanyService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getCompanies', () => {
    it('should fetch companies with default pagination', () => {
      const mockResponse: PagedResult<Company> = {
        items: [
          { id: 1, name: 'Company A', website: 'https://companya.com', location: 'NYC', createdAt: '2024-01-01' },
          { id: 2, name: 'Company B', website: 'https://companyb.com', location: 'LA', createdAt: '2024-01-02' },
        ],
        pageNumber: 1,
        pageSize: 10,
        totalCount: 2,
        totalPages: 1,
      };

      service.getCompanies().subscribe((result) => {
        expect(result.items.length).toBe(2);
        expect(result.items[0].name).toBe('Company A');
        expect(result.totalCount).toBe(2);
      });

      const req = httpMock.expectOne(`${mockApiUrl}?pageNumber=1&pageSize=10`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should fetch companies with custom pagination', () => {
      const mockResponse: PagedResult<Company> = {
        items: [{ id: 3, name: 'Company C', createdAt: '2024-01-03' }],
        pageNumber: 2,
        pageSize: 5,
        totalCount: 10,
        totalPages: 2,
      };

      service.getCompanies(2, 5).subscribe((result) => {
        expect(result.pageNumber).toBe(2);
        expect(result.pageSize).toBe(5);
        expect(result.totalPages).toBe(2);
      });

      const req = httpMock.expectOne(`${mockApiUrl}?pageNumber=2&pageSize=5`);
      expect(req.request.method).toBe('GET');
      req.flush(mockResponse);
    });

    it('should return empty items when no companies exist', () => {
      const mockResponse: PagedResult<Company> = {
        items: [],
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0,
      };

      service.getCompanies().subscribe((result) => {
        expect(result.items.length).toBe(0);
        expect(result.totalCount).toBe(0);
      });

      const req = httpMock.expectOne(`${mockApiUrl}?pageNumber=1&pageSize=10`);
      req.flush(mockResponse);
    });
  });
});
