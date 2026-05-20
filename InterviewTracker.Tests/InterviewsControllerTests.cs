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

    [Fact]
    public async Task GetInterviews_ReturnsOkWithPagedInterviews()
    {
        var mockService = new Mock<IInterviewService>();

        mockService
            .Setup(x => x.GetInterviewsAsync(It.IsAny<PaginationRequest>()))
            .ReturnsAsync(new PagedResult<Interview>
            {
                Items = new List<Interview>
                {
                new Interview { Id = 1, RoleTitle = "Senior .NET Developer" },
                new Interview { Id = 2, RoleTitle = "Backend Developer" }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 2
            });

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.GetInterviews(new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var pagedResult = Assert.IsType<PagedResult<Interview>>(okResult.Value);

        Assert.Equal(2, pagedResult.TotalCount);
        Assert.Equal(2, pagedResult.Items.Count());
    }

    [Fact]
    public async Task UpdateInterview_WhenBusinessValidationFails_ReturnsBadRequest()
    {
        var mockService = new Mock<IInterviewService>();

        mockService
            .Setup(x => x.UpdateInterviewAsync(999, It.IsAny<UpdateInterviewRequest>()))
            .ReturnsAsync((false, "Interview does not exist."));

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.UpdateInterview(999, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 1
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateInterview_WhenValid_ReturnsNoContent()
    {
        var mockService = new Mock<IInterviewService>();

        mockService
            .Setup(x => x.UpdateInterviewAsync(1, It.IsAny<UpdateInterviewRequest>()))
            .ReturnsAsync((true, null));

        var controller = new InterviewsController(mockService.Object);

        var result = await controller.UpdateInterview(1, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Technical Interview",
            CompanyId = 1
        });

        Assert.IsType<NoContentResult>(result);
    }
}