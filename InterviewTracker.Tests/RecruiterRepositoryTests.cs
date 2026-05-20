using InterviewTracker.Data;
using InterviewTracker.Data.Repositories;
using InterviewTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Tests;

public class RecruiterRepositoryTests
{
    private InterviewTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InterviewTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InterviewTrackerDbContext(options);
    }

    [Fact]
    public async Task AddAsync_SavesRecruiter()
    {
        using var context = CreateDbContext();

        var repository = new RecruiterRepository(context);

        var recruiter = new Recruiter
        {
            FullName = "Jane Recruiter",
            Email = "jane@example.com"
        };

        var result = await repository.AddAsync(recruiter);

        Assert.True(result.Id > 0);
        Assert.Equal(1, context.Recruiters.Count());
    }
}