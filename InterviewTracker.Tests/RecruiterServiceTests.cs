using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Services;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Moq;

namespace InterviewTracker.Tests;

public class RecruiterServiceTests
{
    [Fact]
    public async Task CreateRecruiterAsync_CallsRepositoryAndReturnsRecruiter()
    {
        var mockRepository = new Mock<IRecruiterRepository>();

        mockRepository.Setup(x => x.AddAsync(It.IsAny<Recruiter>()))
            .ReturnsAsync((Recruiter recruiter) =>
            {
                recruiter.Id = 1;
                return recruiter;
            });

        var service = new RecruiterService(mockRepository.Object);

        var result = await service.CreateRecruiterAsync(new CreateRecruiterRequest
        {
            FullName = "Jane Recruiter",
            Email = "jane@example.com"
        });

        Assert.Equal(1, result.Id);
        Assert.Equal("Jane Recruiter", result.FullName);

        mockRepository.Verify(x => x.AddAsync(It.IsAny<Recruiter>()), Times.Once);
    }

    [Fact]
    public async Task GetRecruitersAsync_ReturnsPagedRecruiters()
    {
        var mockRepository = new Mock<IRecruiterRepository>();

        mockRepository
            .Setup(x => x.GetPagedAsync(1, 10))
            .ReturnsAsync((
                new List<Recruiter>
                {
                new Recruiter { Id = 1, FullName = "A Recruiter" }
                },
                1
            ));

        var service = new RecruiterService(mockRepository.Object);

        var result = await service.GetRecruitersAsync(new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        });

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
    }
}