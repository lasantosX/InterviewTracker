import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Interview {
  id: number;
  roleTitle: string;
  status: string;
  interviewDate?: string;
  notes?: string;
  expectedSalary?: number;
  companyId: number;
  recruiterId?: number;
  createdAt: string;
  companyName?: string;
  recruiterName?: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

@Injectable({
  providedIn: 'root',
})
export class InterviewService {
  private readonly apiUrl = 'https://localhost:7185/api/Interviews';

  constructor(private http: HttpClient) {}

  getInterviews(
    pageNumber = 1,
    pageSize = 10,
    status?: string,
  ): Observable<PagedResult<Interview>> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get<PagedResult<Interview>>(this.apiUrl, { params });
  }
}
