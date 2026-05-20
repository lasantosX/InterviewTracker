using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly IDashboardSqlQuery _dashboardSqlQuery;

    public DashboardRepository(IDashboardSqlQuery dashboardSqlQuery)
    {
        _dashboardSqlQuery = dashboardSqlQuery;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        return await _dashboardSqlQuery.GetSummaryAsync();
    }
}