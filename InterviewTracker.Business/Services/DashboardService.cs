using InterviewTracker.Business.Interfaces;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<DashboardSummary> GetSummaryAsync()
    {
        return await _dashboardRepository.GetSummaryAsync();
    }
}