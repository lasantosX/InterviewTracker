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

    [Fact]
    public async Task GetPagedAsync_ReturnsPagedRecruitersOrderedByName()
    {
        using var context = CreateDbContext();

        context.Recruiters.AddRange(
            new Recruiter { FullName = "Z Recruiter" },
            new Recruiter { FullName = "A Recruiter" },
            new Recruiter { FullName = "B Recruiter" }
        );

        await context.SaveChangesAsync();

        var repository = new RecruiterRepository(context);

        var result = await repository.GetPagedAsync(1, 2);

        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Items.Count());
        Assert.Equal("A Recruiter", result.Items.First().FullName);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesRecruiter()
    {
        using var context = CreateDbContext();

        var recruiter = new Recruiter
        {
            FullName = "Old Recruiter",
            Email = "old@example.com"
        };

        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        var repository = new RecruiterRepository(context);

        recruiter.FullName = "Updated Recruiter";
        recruiter.Email = "updated@example.com";

        var result = await repository.UpdateAsync(recruiter);

        var updatedRecruiter = await context.Recruiters.FindAsync(recruiter.Id);

        Assert.True(result);
        Assert.NotNull(updatedRecruiter);
        Assert.Equal("Updated Recruiter", updatedRecruiter!.FullName);
        Assert.Equal("updated@example.com", updatedRecruiter.Email);
    }

    [Fact]
    public async Task DeleteAsync_RemovesRecruiter()
    {
        using var context = CreateDbContext();

        var recruiter = new Recruiter
        {
            FullName = "Jane Recruiter"
        };

        context.Recruiters.Add(recruiter);
        await context.SaveChangesAsync();

        var repository = new RecruiterRepository(context);

        var result = await repository.DeleteAsync(recruiter);

        Assert.True(result);
        Assert.Empty(context.Recruiters);
    }
}