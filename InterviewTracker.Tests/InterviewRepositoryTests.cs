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

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedInterviewsOrderedByCreatedAtDescending()
    {
        using var context = CreateDbContext();

        var company = new Company { Name = "TechNova" };
        context.Companies.Add(company);
        await context.SaveChangesAsync();

        context.Interviews.AddRange(
            new Interview
            {
                RoleTitle = "Older Interview",
                Status = "Applied",
                CompanyId = company.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Interview
            {
                RoleTitle = "Newest Interview",
                Status = "Applied",
                CompanyId = company.Id,
                CreatedAt = DateTime.UtcNow
            },
            new Interview
            {
                RoleTitle = "Middle Interview",
                Status = "Applied",
                CompanyId = company.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            }
        );

        await context.SaveChangesAsync();

        var repository = new InterviewRepository(context);

        var result = await repository.GetPagedAsync(1, 2);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal("Newest Interview", result.Items.First().RoleTitle);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesInterview()
    {
        using var context = CreateDbContext();

        var company = new Company { Name = "TechNova" };
        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var interview = new Interview
        {
            RoleTitle = "Old Role",
            Status = "Applied",
            CompanyId = company.Id
        };

        context.Interviews.Add(interview);
        await context.SaveChangesAsync();

        var repository = new InterviewRepository(context);

        interview.RoleTitle = "Updated Role";
        interview.Status = "Technical Interview";

        var result = await repository.UpdateAsync(interview);

        var updatedInterview = await context.Interviews.FindAsync(interview.Id);

        Assert.True(result);
        Assert.NotNull(updatedInterview);
        Assert.Equal("Updated Role", updatedInterview!.RoleTitle);
        Assert.Equal("Technical Interview", updatedInterview.Status);
    }
}