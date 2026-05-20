using InterviewTracker.Data;
using InterviewTracker.Data.Repositories;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Tests;

public class InterviewRepositoryTests
{
    private InterviewTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InterviewTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InterviewTrackerDbContext(options);
    }

    [Fact]
    public async Task AddAsync_SavesInterview()
    {
        using var context = CreateDbContext();

        context.Companies.Add(new Company
        {
            Id = 1,
            Name = "TechNova"
        });

        await context.SaveChangesAsync();

        var repository = new InterviewRepository(context);

        var interview = new Interview
        {
            RoleTitle = "Senior .NET Developer",
            Status = "Applied",
            CompanyId = 1
        };

        var result = await repository.AddAsync(interview);

        Assert.True(result.Id > 0);
        Assert.Equal(1, context.Interviews.Count());
    }

    [Fact]
    public async Task CompanyExistsAsync_WhenCompanyExists_ReturnsTrue()
    {
        using var context = CreateDbContext();

        context.Companies.Add(new Company
        {
            Name = "TechNova"
        });

        await context.SaveChangesAsync();

        var companyId = context.Companies.First().Id;

        var repository = new InterviewRepository(context);

        var result = await repository.CompanyExistsAsync(companyId);

        Assert.True(result);
    }
}