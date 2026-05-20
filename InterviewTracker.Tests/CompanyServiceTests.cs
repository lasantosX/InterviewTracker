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
}