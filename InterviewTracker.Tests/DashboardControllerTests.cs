using InterviewTracker.Api.Controllers;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewTracker.Tests;

public class DashboardControllerTests
{
    [Fact]
    public async Task GetSummary_ReturnsOkWithDashboardSummary()
    {
        var mockService = new Mock<IDashboardService>();

        mockService.Setup(x => x.GetSummaryAsync())
            .ReturnsAsync(new DashboardSummary
            {
                TotalCompanies = 2,
                TotalRecruiters = 1,
                TotalInterviews = 3
            });

        var controller = new DashboardController(mockService.Object);

        var result = await controller.GetSummary();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var summary = Assert.IsType<DashboardSummary>(okResult.Value);

        Assert.Equal(2, summary.TotalCompanies);
        Assert.Equal(3, summary.TotalInterviews);
    }
}