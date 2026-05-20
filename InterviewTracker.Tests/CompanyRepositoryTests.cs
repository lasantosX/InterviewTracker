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

    [Fact]
    public async Task UpdateAsync_UpdatesCompany()
    {
        using var context = CreateDbContext();

        var company = new Company
        {
            Name = "Old Company",
            Location = "Old Location"
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var repository = new CompanyRepository(context);

        company.Name = "Updated Company";
        company.Location = "Remote";

        var result = await repository.UpdateAsync(company);

        var updatedCompany = await context.Companies.FindAsync(company.Id);

        Assert.True(result);
        Assert.NotNull(updatedCompany);
        Assert.Equal("Updated Company", updatedCompany!.Name);
        Assert.Equal("Remote", updatedCompany.Location);
    }

    [Fact]
    public async Task DeleteAsync_RemovesCompany()
    {
        using var context = CreateDbContext();

        var company = new Company
        {
            Name = "TechNova"
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var repository = new CompanyRepository(context);

        var result = await repository.DeleteAsync(company);

        Assert.True(result);
        Assert.Empty(context.Companies);
    }
}