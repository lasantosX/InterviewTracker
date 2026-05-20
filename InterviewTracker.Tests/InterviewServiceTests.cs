using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Services;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using Moq;

namespace InterviewTracker.Tests;

public class InterviewServiceTests
{
    [Fact]
    public async Task CreateInterviewAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository.Setup(x => x.CompanyExistsAsync(999))
            .ReturnsAsync(false);

        var service = new InterviewService(mockRepository.Object);

        var result = await service.CreateInterviewAsync(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            CompanyId = 999
        });

        Assert.False(result.Success);
        Assert.Equal("Company does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateInterviewAsync_WhenRecruiterDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository.Setup(x => x.CompanyExistsAsync(1))
            .ReturnsAsync(true);

        mockRepository.Setup(x => x.RecruiterExistsAsync(999))
            .ReturnsAsync(false);

        var service = new InterviewService(mockRepository.Object);

        var result = await service.CreateInterviewAsync(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            CompanyId = 1,
            RecruiterId = 999
        });

        Assert.False(result.Success);
        Assert.Equal("Recruiter does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateInterviewAsync_WhenValid_CallsRepositoryAndReturnsSuccess()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository.Setup(x => x.CompanyExistsAsync(1))
            .ReturnsAsync(true);

        mockRepository.Setup(x => x.AddAsync(It.IsAny<Interview>()))
            .ReturnsAsync((Interview interview) =>
            {
                interview.Id = 1;
                return interview;
            });

        var service = new InterviewService(mockRepository.Object);

        var result = await service.CreateInterviewAsync(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 1
        });

        Assert.True(result.Success);
        Assert.NotNull(result.Interview);
        Assert.Equal(1, result.Interview!.Id);

        mockRepository.Verify(x => x.AddAsync(It.IsAny<Interview>()), Times.Once);
    }

    [Fact]
    public async Task CreateInterviewAsync_WhenStatusIsInvalid_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        var service = new InterviewService(mockRepository.Object);

        var result = await service.CreateInterviewAsync(new CreateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Random Status",
            CompanyId = 1
        });

        Assert.False(result.Success);
        Assert.Equal("Invalid interview status.", result.ErrorMessage);
    }
}