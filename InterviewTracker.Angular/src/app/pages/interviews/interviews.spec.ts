import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of } from 'rxjs';

import { Interviews } from './interviews';
import { InterviewService } from '../../services/interview';
import { CompanyService } from '../../services/company';

describe('Interviews', () => {
  let component: Interviews;
  let fixture: ComponentFixture<Interviews>;

  const interviewServiceMock = {
    getInterviews: vi.fn().mockReturnValue(
      of({
        items: [],
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0,
      }),
    ),
    createInterview: vi.fn(),
  };

  const companyServiceMock = {
    getCompanies: vi.fn().mockReturnValue(
      of({
        items: [],
        pageNumber: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0,
      }),
    ),
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Interviews],
      providers: [
        { provide: InterviewService, useValue: interviewServiceMock },
        { provide: CompanyService, useValue: companyServiceMock },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Interviews);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
