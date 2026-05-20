using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IDashboardSqlQuery
{
    Task<DashboardSummary> GetSummaryAsync();
}