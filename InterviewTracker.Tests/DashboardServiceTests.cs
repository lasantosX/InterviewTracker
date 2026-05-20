using InterviewTracker.Business.Services;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Moq;

namespace InterviewTracker.Tests;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetSummaryAsync_ReturnsSummaryFromRepository()
    {
        var mockRepository = new Mock<IDashboardRepository>();

        mockRepository.Setup(x => x.GetSummaryAsync())
            .ReturnsAsync(new DashboardSummary
            {
                TotalCompanies = 2,
                TotalRecruiters = 1,
                TotalInterviews = 3
            });

        var service = new DashboardService(mockRepository.Object);

        var result = await service.GetSummaryAsync();

        Assert.Equal(2, result.TotalCompanies);
        Assert.Equal(1, result.TotalRecruiters);

        mockRepository.Verify(x => x.GetSummaryAsync(), Times.Once);
    }
}