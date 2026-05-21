import { Component, OnInit } from '@angular/core';
import { Company, CompanyService } from '../../services/company';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-companies',
  imports: [DatePipe],
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
}
