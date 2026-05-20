using InterviewTracker.Data;
using InterviewTracker.Data.Repositories;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Tests;

public class CompanyRepositoryTests
{
    private InterviewTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InterviewTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InterviewTrackerDbContext(options);
    }

    [Fact]
    public async Task AddAsync_SavesCompany()
    {
        using var context = CreateDbContext();

        var repository = new CompanyRepository(context);

        var company = new Company
        {
            Name = "TechNova",
            Location = "Remote"
        };

        var result = await repository.AddAsync(company);

        Assert.True(result.Id > 0);
        Assert.Equal(1, context.Companies.Count());
    }

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedCompaniesOrderedByName()
    {
        using var context = CreateDbContext();

        context.Companies.AddRange(
            new Company { Name = "Z Company" },
            new Company { Name = "A Company" },
            new Company { Name = "B Company" }
        );

        await context.SaveChangesAsync();

        var repository = new CompanyRepository(context);

        var result = await repository.GetPagedAsync(1, 2);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal("A Company", result.Items.First().Name);
    }
}