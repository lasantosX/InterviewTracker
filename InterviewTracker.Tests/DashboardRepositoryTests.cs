using InterviewTracker.Data;
using InterviewTracker.Data.Repositories;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Tests;

public class DashboardRepositoryTests
{
    private InterviewTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InterviewTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InterviewTrackerDbContext(options);
    }

    [Fact]
    public async Task GetSummaryAsync_ReturnsCorrectCounts()
    {
        using var context = CreateDbContext();

        var company = new Company { Name = "TechNova" };
        var recruiter = new Recruiter { FullName = "Jane Recruiter" };

        context.Companies.Add(company);
        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        context.Interviews.AddRange(
            new Interview
            {
                RoleTitle = "Senior .NET Developer",
                Status = "Applied",
                CompanyId = company.Id,
                ExpectedSalary = 4000
            },
            new Interview
            {
                RoleTitle = "Backend Developer",
                Status = "Technical Interview",
                CompanyId = company.Id,
                RecruiterId = recruiter.Id,
                ExpectedSalary = 5000
            }
        );

        await context.SaveChangesAsync();

        var repository = new DashboardRepository(context);

        var result = await repository.GetSummaryAsync();

        Assert.Equal(1, result.TotalCompanies);
        Assert.Equal(1, result.TotalRecruiters);
        Assert.Equal(2, result.TotalInterviews);
        Assert.Equal(1, result.AppliedCount);
        Assert.Equal(1, result.TechnicalInterviewCount);
        Assert.Equal(4500, result.AverageExpectedSalary);
    }
}