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

    [Fact]
    public async Task GetCompanies_ReturnsOkWithPagedCompanies()
    {
        var mockService = new Mock<ICompanyService>();

        mockService
            .Setup(x => x.GetCompaniesAsync(It.IsAny<PaginationRequest>()))
            .ReturnsAsync(new PagedResult<Company>
            {
                Items = new List<Company>
                {
                new Company { Id = 1, Name = "A Company" },
                new Company { Id = 2, Name = "B Company" }
                },
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 2
            });

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.GetCompanies(new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        });

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var pagedResult = Assert.IsType<PagedResult<Company>>(okResult.Value);

        Assert.Equal(2, pagedResult.TotalCount);
        Assert.Equal(2, pagedResult.Items.Count());
        Assert.Equal(1, pagedResult.PageNumber);
        Assert.Equal(10, pagedResult.PageSize);
    }

    [Fact]
    public async Task UpdateCompany_WhenCompanyDoesNotExist_ReturnsNotFound()
    {
        var mockService = new Mock<ICompanyService>();

        mockService
            .Setup(x => x.UpdateCompanyAsync(999, It.IsAny<UpdateCompanyRequest>()))
            .ReturnsAsync((false, "Company does not exist."));

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.UpdateCompany(999, new UpdateCompanyRequest
        {
            Name = "Updated Company"
        });

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCompany_WhenValid_ReturnsNoContent()
    {
        var mockService = new Mock<ICompanyService>();

        mockService
            .Setup(x => x.UpdateCompanyAsync(1, It.IsAny<UpdateCompanyRequest>()))
            .ReturnsAsync((true, null));

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.UpdateCompany(1, new UpdateCompanyRequest
        {
            Name = "Updated Company"
        });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCompany_WhenCompanyDoesNotExist_ReturnsNotFound()
    {
        var mockService = new Mock<ICompanyService>();

        mockService
            .Setup(x => x.DeleteCompanyAsync(999))
            .ReturnsAsync((false, "Company does not exist."));

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.DeleteCompany(999);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCompany_WhenValid_ReturnsNoContent()
    {
        var mockService = new Mock<ICompanyService>();

        mockService
            .Setup(x => x.DeleteCompanyAsync(1))
            .ReturnsAsync((true, null));

        var controller = new CompaniesController(mockService.Object);

        var result = await controller.DeleteCompany(1);

        Assert.IsType<NoContentResult>(result);
    }
}