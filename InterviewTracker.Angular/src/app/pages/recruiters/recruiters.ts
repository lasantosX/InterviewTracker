import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Recruiter, RecruiterService } from '../../services/recruiter';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-recruiters',
  imports: [DatePipe, FormsModule],
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

  showCreateForm = false;
  editingRecruiterId?: number;

  newRecruiter = {
    fullName: '',
    email: '',
    linkedInUrl: '',
  };

  editRecruiter = {
    fullName: '',
    email: '',
    linkedInUrl: '',
  };

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

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
  }

  createRecruiter(): void {
    this.recruiterService.createRecruiter(this.newRecruiter).subscribe({
      next: () => {
        this.showCreateForm = false;
        this.newRecruiter = {
          fullName: '',
          email: '',
          linkedInUrl: '',
        };
        this.loadRecruiters();
      },
      error: () => {
        this.errorMessage = 'Unable to create recruiter.';
      },
    });
  }

  startEdit(recruiter: Recruiter): void {
    this.editingRecruiterId = recruiter.id;

    this.editRecruiter = {
      fullName: recruiter.fullName,
      email: recruiter.email || '',
      linkedInUrl: recruiter.linkedInUrl || '',
    };
  }

  cancelEdit(): void {
    this.editingRecruiterId = undefined;
  }

  updateRecruiter(): void {
    if (!this.editingRecruiterId) {
      return;
    }

    this.recruiterService.updateRecruiter(this.editingRecruiterId, this.editRecruiter).subscribe({
      next: () => {
        this.editingRecruiterId = undefined;
        this.loadRecruiters();
      },
      error: () => {
        this.errorMessage = 'Unable to update recruiter.';
      },
    });
  }

  deleteRecruiter(id: number): void {
    const confirmed = confirm('Delete this recruiter?\n\nThis action cannot be undone.');

    if (!confirmed) {
      return;
    }

    this.recruiterService.deleteRecruiter(id).subscribe({
      next: () => this.loadRecruiters(),
      error: () => {
        this.errorMessage = 'Unable to delete recruiter.';
      },
    });
  }
}
