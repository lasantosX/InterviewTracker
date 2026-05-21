import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Interview, InterviewService } from '../../services/interview';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-interviews',
  imports: [FormsModule, DatePipe],
  templateUrl: './interviews.html',
  styleUrl: './interviews.scss',
})
export class Interviews implements OnInit {
  interviews: Interview[] = [];
  status = '';
  pageNumber = 1;
  pageSize = 10;
  totalPages = 0;
  isLoading = false;
  errorMessage = '';

  statuses = ['', 'Applied', 'Recruiter Screen', 'Technical Interview', 'Offer', 'Rejected'];

  constructor(private interviewService: InterviewService) {}

  ngOnInit(): void {
    this.loadInterviews();
  }

  loadInterviews(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.interviewService
      .getInterviews(this.pageNumber, this.pageSize, this.status || undefined)
      .subscribe({
        next: (result) => {
          this.interviews = result.items;
          this.totalPages = result.totalPages;
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'Unable to load interviews.';
          this.isLoading = false;
        },
      });
  }

  applyFilter(): void {
    this.pageNumber = 1;
    this.loadInterviews();
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadInterviews();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadInterviews();
    }
  }
}
