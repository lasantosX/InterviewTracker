import { Component, OnInit } from '@angular/core';
import { DashboardService, DashboardSummary } from '../../services/dashboard';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements OnInit {
  summary?: DashboardSummary;
  isLoading = true;
  errorMessage = '';

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getSummary().subscribe({
      next: (data) => {
        this.summary = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load dashboard summary.';
        this.isLoading = false;
      },
    });
  }
}
