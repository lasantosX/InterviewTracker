using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface IDashboardService
{
    Task<DashboardSummary> GetSummaryAsync();
}