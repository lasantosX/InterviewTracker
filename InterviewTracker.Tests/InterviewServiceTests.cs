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

    [Fact]
    public async Task GetInterviewsAsync_ReturnsPagedInterviews()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository
            .Setup(x => x.GetPagedAsync(1, 10))
            .ReturnsAsync((
                new List<Interview>
                {
                new Interview { Id = 1, RoleTitle = "Senior .NET Developer" }
                },
                1
            ));

        var service = new InterviewService(mockRepository.Object);

        var result = await service.GetInterviewsAsync(new PaginationRequest
        {
            PageNumber = 1,
            PageSize = 10
        });

        Assert.Single(result.Items);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task UpdateInterviewAsync_WhenStatusIsInvalid_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        var service = new InterviewService(mockRepository.Object);

        var result = await service.UpdateInterviewAsync(1, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Invalid Status",
            CompanyId = 1
        });

        Assert.False(result.Success);
        Assert.Equal("Invalid interview status.", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateInterviewAsync_WhenInterviewDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository
            .Setup(x => x.GetByIdAsync(999))
            .ReturnsAsync((Interview?)null);

        var service = new InterviewService(mockRepository.Object);

        var result = await service.UpdateInterviewAsync(999, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 1
        });

        Assert.False(result.Success);
        Assert.Equal("Interview does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateInterviewAsync_WhenCompanyDoesNotExist_ReturnsFailure()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        mockRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new Interview
            {
                Id = 1,
                RoleTitle = "Old Role",
                CompanyId = 1
            });

        mockRepository
            .Setup(x => x.CompanyExistsAsync(999))
            .ReturnsAsync(false);

        var service = new InterviewService(mockRepository.Object);

        var result = await service.UpdateInterviewAsync(1, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 999
        });

        Assert.False(result.Success);
        Assert.Equal("Company does not exist.", result.ErrorMessage);
    }

    [Fact]
    public async Task UpdateInterviewAsync_WhenValid_UpdatesInterview()
    {
        var mockRepository = new Mock<IInterviewRepository>();

        var interview = new Interview
        {
            Id = 1,
            RoleTitle = "Old Role",
            Status = "Applied",
            CompanyId = 1
        };

        mockRepository
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(interview);

        mockRepository
            .Setup(x => x.CompanyExistsAsync(1))
            .ReturnsAsync(true);

        mockRepository
            .Setup(x => x.UpdateAsync(It.IsAny<Interview>()))
            .ReturnsAsync(true);

        var service = new InterviewService(mockRepository.Object);

        var result = await service.UpdateInterviewAsync(1, new UpdateInterviewRequest
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Technical Interview",
            CompanyId = 1,
            ExpectedSalary = 4500,
            Notes = "Updated notes"
        });

        Assert.True(result.Success);
        Assert.Equal("Senior .NET Developer", interview.RoleTitle);
        Assert.Equal("Technical Interview", interview.Status);
        Assert.Equal(4500, interview.ExpectedSalary);
        Assert.Equal("Updated notes", interview.Notes);

        mockRepository.Verify(x => x.UpdateAsync(interview), Times.Once);
    }
}