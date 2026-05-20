using InterviewTracker.Data.Interfaces;
using InterviewTracker.Data.Repositories;
using InterviewTracker.Domain.Models;
using Moq;

namespace InterviewTracker.Tests;

public class DashboardRepositoryTests
{
    [Fact]
    public async Task GetSummaryAsync_ReturnsSummaryFromSqlQuery()
    {
        var mockSqlQuery = new Mock<IDashboardSqlQuery>();

        mockSqlQuery.Setup(x => x.GetSummaryAsync())
            .ReturnsAsync(new DashboardSummary
            {
                TotalCompanies = 1,
                TotalRecruiters = 1,
                TotalInterviews = 2,
                AppliedCount = 1,
                TechnicalInterviewCount = 1,
                AverageExpectedSalary = 4500
            });

        var repository = new DashboardRepository(mockSqlQuery.Object);

        var result = await repository.GetSummaryAsync();

        Assert.Equal(1, result.TotalCompanies);
        Assert.Equal(2, result.TotalInterviews);
        Assert.Equal(4500, result.AverageExpectedSalary);

        mockSqlQuery.Verify(x => x.GetSummaryAsync(), Times.Once);
    }
}