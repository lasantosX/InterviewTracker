using InterviewTracker.Api.Controllers;
using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewTracker.Tests;

public class InterviewsControllerTests
{
    [Fact]
    public async Task GetInterviewById_WhenInterviewDoesNotExist_ReturnsNotFound()
    {
        var mockService = new Mock<IInterviewService>();

        mockService.Setup(x => x.GetInterviewByIdAsync(999))
            .ReturnsAsync((Interview?)null);

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.GetInterviewById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateInterview_WhenBusinessValidationFails_ReturnsBadRequest()
    {
        var mockService = new Mock<IInterviewService>();

        mockService.Setup(x => x.CreateInterviewAsync(It.IsAny<CreateInterviewRequest>()))
            .ReturnsAsync((false, "Company does not exist.", null));

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.CreateInterview(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            CompanyId = 999
        });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateInterview_WhenValid_ReturnsCreatedAtAction()
    {
        var mockService = new Mock<IInterviewService>();

        var interview = new Interview
        {
            Id = 1,
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 1
        };

        mockService.Setup(x => x.CreateInterviewAsync(It.IsAny<CreateInterviewRequest>()))
            .ReturnsAsync((true, null, interview));

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.CreateInterview(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            CompanyId = 1
        });

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}