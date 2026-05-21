import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Recruiter, RecruiterService } from '../../services/recruiter';

@Component({
  selector: 'app-recruiters',
  imports: [DatePipe],
  templateUrl: './recruiters.html',
  styleUrl: './recruiters.scss',
})
export class Recruiters implements OnInit {
  recruiters: Recruiter[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalPages = 0;
  isLoading = false;
  errorMessage = '';

  constructor(private recruiterService: RecruiterService) {}

  ngOnInit(): void {
    this.loadRecruiters();
  }

  loadRecruiters(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.recruiterService.getRecruiters(this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.recruiters = result.items;
        this.totalPages = result.totalPages;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load recruiters.';
        this.isLoading = false;
      },
    });
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadRecruiters();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadRecruiters();
    }
  }
}
