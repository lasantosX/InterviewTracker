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

    [Fact]
    public async Task UpdateCompanyAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        mockRepository
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Company?)null);

        var service = new CompanyService(mockRepository.Object);

        var result = await service.UpdateCompanyAsync(999, new UpdateCompanyRequest
        {
            Name = "Updated Company"
        });

        Assert.False(result.Success);
        Assert.Equal("Company does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateCompanyAsync_WhenCompanyExists_UpdatesCompany()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        var company = new Company
        {
            Id = 1,
            Name = "Old Company"
        };

        mockRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(company);

        mockRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Company>()))
            .ReturnsAsync(true);

        var service = new CompanyService(mockRepository.Object);

        var result = await service.UpdateCompanyAsync(1, new UpdateCompanyRequest
        {
            Name = "Updated Company",
            Website = "https://example.com",
            Location = "Remote"
        });

        Assert.True(result.Success);
        Assert.Equal("Updated Company", company.Name);
        Assert.Equal("https://example.com", company.Website);
        Assert.Equal("Remote", company.Location);

        mockRepository.Verify(x => x.UpdateAsync(company), Times.Once);
    }

    [Fact]
    public async Task DeleteCompanyAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        mockRepository
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Company?)null);

        var service = new CompanyService(mockRepository.Object);

        var result = await service.DeleteCompanyAsync(999);

        Assert.False(result.Success);
        Assert.Equal("Company does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task DeleteCompanyAsync_WhenCompanyExists_DeletesCompany()
    {
        var mockRepository = new Mock<ICompanyRepository>();

        var company = new Company
        {
            Id = 1,
            Name = "TechNova"
        };

        mockRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(company);

        mockRepository
            .Setup(x => x.DeleteAsync(company))
            .ReturnsAsync(true);

        var service = new CompanyService(mockRepository.Object);

        var result = await service.DeleteCompanyAsync(1);

        Assert.True(result.Success);

        mockRepository.Verify(x => x.DeleteAsync(company), Times.Once);
    }
}