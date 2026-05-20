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
}