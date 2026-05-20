using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Data.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly InterviewTrackerDbContext _context;

    public DashboardRepository(InterviewTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        var totalCompanies = await _context.Companies.CountAsync();
        var totalRecruiters = await _context.Recruiters.CountAsync();
        var totalInterviews = await _context.Interviews.CountAsync();

        var appliedCount = await _context.Interviews
            .CountAsync(x => x.Status == "Applied");

        var technicalInterviewCount = await _context.Interviews
            .CountAsync(x => x.Status == "Technical Interview");

        var offerCount = await _context.Interviews
            .CountAsync(x => x.Status == "Offer");

        var rejectedCount = await _context.Interviews
            .CountAsync(x => x.Status == "Rejected");

        var averageExpectedSalary = await _context.Interviews
            .Where(x => x.ExpectedSalary.HasValue)
            .AverageAsync(x => (decimal?)x.ExpectedSalary) ?? 0;

        return new DashboardSummary
        {
            TotalCompanies = totalCompanies,
            TotalRecruiters = totalRecruiters,
            TotalInterviews = totalInterviews,
            AppliedCount = appliedCount,
            TechnicalInterviewCount = technicalInterviewCount,
            OfferCount = offerCount,
            RejectedCount = rejectedCount,
            AverageExpectedSalary = averageExpectedSalary
        };
    }
}