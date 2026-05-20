using InterviewTracker.Api.Controllers;
using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace InterviewTracker.Tests;

public class CompaniesControllerTests
{
    [Fact]
    public async Task GetCompanyById_WhenCompanyDoesNotExist_ReturnsNotFound()
    {
        var mockService = new Mock<ICompanyService>();

        mockService.Setup(x => x.GetCompanyByIdAsync(999))
            .ReturnsAsync((Company?)null);

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.GetCompanyById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCompany_ReturnsCreatedAtAction()
    {
        var mockService = new Mock<ICompanyService>();

        var request = new CreateCompanyRequest { Name = "TechNova" };
        var company = new Company { Id = 1, Name = "TechNova" };

        mockService.Setup(x => x.CreateCompanyAsync(It.IsAny<CreateCompanyRequest>()))
            .ReturnsAsync(company);

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.CreateCompany(request);

        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
}