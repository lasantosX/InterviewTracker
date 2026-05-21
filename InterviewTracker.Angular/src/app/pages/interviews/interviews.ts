import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Interview, InterviewService } from '../../services/interview';
import { DatePipe } from '@angular/common';
import { Company, CompanyService } from '../../services/company';
import { Recruiter, RecruiterService } from '../../services/recruiter';

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
  companies: Company[] = [];
  recruiters: Recruiter[] = [];

  showCreateForm = false;

  newInterview = {
    roleTitle: '',
    status: 'Applied',
    interviewDate: '',
    notes: '',
    expectedSalary: 0,
    companyId: 1,
    recruiterId: undefined as number | undefined,
  };

  statuses = ['', 'Applied', 'Recruiter Screen', 'Technical Interview', 'Offer', 'Rejected'];

  constructor(
    private interviewService: InterviewService,
    private companyService: CompanyService,
    private recruiterService: RecruiterService,
  ) {}

  ngOnInit(): void {
    this.loadCompanies();
    this.loadRecruiters();
    this.loadInterviews();
  }

  loadRecruiters(): void {
    this.recruiterService.getRecruiters(1, 100).subscribe({
      next: (result) => {
        this.recruiters = result.items;
      },
    });
  }

  loadCompanies(): void {
    this.companyService.getCompanies(1, 100).subscribe({
      next: (result) => {
        this.companies = result.items;

        if (this.companies.length > 0) {
          this.newInterview.companyId = this.companies[0].id;
        }
      },
    });
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

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
  }

  createInterview(): void {
    this.interviewService
      .createInterview({
        roleTitle: this.newInterview.roleTitle,
        status: this.newInterview.status,
        interviewDate: this.newInterview.interviewDate || undefined,
        notes: this.newInterview.notes || undefined,
        expectedSalary: this.newInterview.expectedSalary || undefined,
        companyId: this.newInterview.companyId,
        recruiterId: this.newInterview.recruiterId,
      })
      .subscribe({
        next: () => {
          this.showCreateForm = false;
          this.loadInterviews();
        },
        error: () => {
          this.errorMessage = 'Unable to create interview.';
        },
      });
  }

  deleteInterview(id: number): void {
    const confirmed = confirm('Are you sure you want to delete this interview?');

    if (!confirmed) {
      return;
    }

    this.interviewService.deleteInterview(id).subscribe({
      next: () => this.loadInterviews(),
      error: () => {
        this.errorMessage = 'Unable to delete interview.';
      },
    });
  }
}
