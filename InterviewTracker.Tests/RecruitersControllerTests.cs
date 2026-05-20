using InterviewTracker.Api.Controllers;
using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewTracker.Tests;

public class RecruitersControllerTests
{
    [Fact]
    public async Task GetRecruiterById_WhenRecruiterDoesNotExist_ReturnsNotFound()
    {
        var mockService = new Mock<IRecruiterService>();

        mockService.Setup(x => x.GetRecruiterByIdAsync(999))
            .ReturnsAsync((Recruiter?)null);

        var controller = new RecruitersController(mockService.Object);

        var result = await controller.GetRecruiterById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateRecruiter_ReturnsCreatedAtAction()
    {
        var mockService = new Mock<IRecruiterService>();

        var recruiter = new Recruiter
        {
            Id = 1,
            FullName = "Jane Recruiter",
            Email = "jane@example.com"
        };

        mockService.Setup(x => x.CreateRecruiterAsync(It.IsAny<CreateRecruiterRequest>()))
            .ReturnsAsync(recruiter);

        var controller = new RecruitersController(mockService.Object);

        var result = await controller.CreateRecruiter(new CreateRecruiterRequest
        {
            FullName = "Jane Recruiter",
            Email = "jane@example.com"
        });

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}