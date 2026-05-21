import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Company {
  id: number;
  name: string;
  website?: string;
  location?: string;
  createdAt: string;
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
export class CompanyService {
  private readonly apiUrl = 'https://localhost:7185/api/Companies';

  constructor(private http: HttpClient) {}

  getCompanies(pageNumber = 1, pageSize = 10): Observable<PagedResult<Company>> {
    const params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);

    return this.http.get<PagedResult<Company>>(this.apiUrl, { params });
  }
}
