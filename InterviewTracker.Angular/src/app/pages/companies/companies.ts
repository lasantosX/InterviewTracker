import { Component, OnInit } from '@angular/core';
import { Company, CompanyService } from '../../services/company';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-companies',
  imports: [DatePipe, FormsModule],
  templateUrl: './companies.html',
  styleUrl: './companies.scss',
})
export class Companies implements OnInit {
  companies: Company[] = [];
  pageNumber = 1;
  pageSize = 10;
  totalPages = 0;
  isLoading = false;
  errorMessage = '';

  showCreateForm = false;

  editingCompanyId?: number;

  newCompany = {
    name: '',
    website: '',
    location: '',
  };

  editCompany = {
    name: '',
    website: '',
    location: '',
  };

  constructor(private companyService: CompanyService) {}

  ngOnInit(): void {
    this.loadCompanies();
  }

  loadCompanies(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.companyService.getCompanies(this.pageNumber, this.pageSize).subscribe({
      next: (result) => {
        this.companies = result.items;
        this.totalPages = result.totalPages;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Unable to load companies.';
        this.isLoading = false;
      },
    });
  }

  nextPage(): void {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber++;
      this.loadCompanies();
    }
  }

  previousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadCompanies();
    }
  }

  toggleCreateForm(): void {
    this.showCreateForm = !this.showCreateForm;
  }

  createCompany(): void {
    this.companyService.createCompany(this.newCompany).subscribe({
      next: () => {
        this.showCreateForm = false;

        this.newCompany = {
          name: '',
          website: '',
          location: '',
        };

        this.loadCompanies();
      },
      error: () => {
        this.errorMessage = 'Unable to create company.';
      },
    });
  }

  startEdit(company: Company): void {
    this.editingCompanyId = company.id;

    this.editCompany = {
      name: company.name,
      website: company.website || '',
      location: company.location || '',
    };
  }

  cancelEdit(): void {
    this.editingCompanyId = undefined;
  }

  updateCompany(): void {
    if (!this.editingCompanyId) {
      return;
    }

    this.companyService.updateCompany(this.editingCompanyId, this.editCompany).subscribe({
      next: () => {
        this.editingCompanyId = undefined;
        this.loadCompanies();
      },
      error: () => {
        this.errorMessage = 'Unable to update company.';
      },
    });
  }

  deleteCompany(id: number): void {
    const confirmed = confirm('Delete this company?\n\nThis action cannot be undone.');

    if (!confirmed) {
      return;
    }

    this.companyService.deleteCompany(id).subscribe({
      next: () => this.loadCompanies(),
      error: () => {
        this.errorMessage = 'Unable to delete company.';
      },
    });
  }
}
