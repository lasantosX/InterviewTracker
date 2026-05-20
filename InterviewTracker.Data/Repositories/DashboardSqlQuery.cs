using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Data.Repositories;

public class DashboardSqlQuery : IDashboardSqlQuery
{
    private readonly InterviewTrackerDbContext _context;

    public DashboardSqlQuery(InterviewTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        var summaries = await _context.DashboardSummaries
            .FromSqlRaw("EXEC dbo.GetInterviewDashboardSummary")
            .AsNoTracking()
            .ToListAsync();

        return summaries.FirstOrDefault() ?? new DashboardSummary();
    }
}