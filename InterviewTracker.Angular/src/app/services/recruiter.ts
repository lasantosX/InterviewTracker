import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Recruiter {
  id: number;
  fullName: string;
  email?: string;
  linkedInUrl?: string;
  createdAt: string;
}

export interface PagedResult<T> {
  items: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface CreateRecruiterRequest {
  fullName: string;
  email?: string;
  linkedInUrl?: string;
}

export interface UpdateRecruiterRequest {
  fullName: string;
  email?: string;
  linkedInUrl?: string;
}

@Injectable({
  providedIn: 'root',
})
export class RecruiterService {
  private readonly apiUrl = 'https://localhost:7185/api/Recruiters';

  constructor(private http: HttpClient) {}

  getRecruiters(pageNumber = 1, pageSize = 10): Observable<PagedResult<Recruiter>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);

    return this.http.get<PagedResult<Recruiter>>(this.apiUrl, { params });
  }

  createRecruiter(request: CreateRecruiterRequest): Observable<Recruiter> {
    return this.http.post<Recruiter>(this.apiUrl, request);
  }

  updateRecruiter(id: number, request: UpdateRecruiterRequest): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, request);
  }

  deleteRecruiter(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
