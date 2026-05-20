using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardSummary> GetSummaryAsync();
}