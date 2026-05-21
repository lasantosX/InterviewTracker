import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface DashboardSummary {
  totalCompanies: number;
  totalRecruiters: number;
  totalInterviews: number;
  appliedCount: number;
  technicalInterviewCount: number;
  offerCount: number;
  rejectedCount: number;
  averageExpectedSalary: number;
}

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private readonly apiUrl = 'https://localhost:7185/api/Dashboard';

  constructor(private http: HttpClient) {}

  getSummary(): Observable<DashboardSummary> {
    return this.http.get<DashboardSummary>(`${this.apiUrl}/summary`);
  }
}
