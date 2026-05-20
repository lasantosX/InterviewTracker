using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Services;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Moq;

namespace InterviewTracker.Tests;

public class CompanyServiceTests
{
    [Fact]
    public async Task CreateCompanyAsync_CallsRepositoryAndReturnsCompany()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        mockRepository.Setup(x => x.AddAsync(It.IsAny<Company>()))
            .ReturnsAsync((Company company) =>
            {
                company.Id = 1;
                return company;
            });

        var service = new CompanyService(mockRepository.Object);

        var result = await service.CreateCompanyAsync(new CreateCompanyRequest
        {
            Name = "TechNova",
            Location = "Remote"
        });

        Assert.Equal(1, result.Id);
        Assert.Equal("TechNova", result.Name);

        mockRepository.Verify(x => x.AddAsync(It.IsAny<Company>()), Times.Once);
    }

    [Fact]
    public async Task GetCompaniesAsync_ReturnsPagedCompanies()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        mockRepository
            .Setup(x => x.GetPagedAsync(1, 10))
            .ReturnsAsync((
                new List<Company>
                {
                new Company { Id = 1, Name = "A Company" }
                },
                1
            ));

        var service = new CompanyService(mockRepository.Object);

        var result = await service.GetCompaniesAsync(new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        });

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
    }
}