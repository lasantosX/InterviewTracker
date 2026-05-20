using InterviewTracker.Api.Controllers;
using InterviewTracker.Api.Dtos;
using InterviewTracker.Data;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Tests;

public class CompaniesControllerTests
{
    private InterviewTrackerDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<InterviewTrackerDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new InterviewTrackerDbContext(options);
    }

    [Fact]
    public async Task GetCompanies_ReturnsCompaniesOrderedByName()
    {
        using var context = CreateDbContext();

        context.Companies.AddRange(
            new Company { Name = "Z Company", Location = "USA" },
            new Company { Name = "A Company", Location = "Remote" }
        );

        await context.SaveChangesAsync();

        var controller = new CompaniesController(context);

        var result = await controller.GetCompanies();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var companies = Assert.IsAssignableFrom<IEnumerable<Company>>(okResult.Value);

        Assert.Equal(2, companies.Count());
        Assert.Equal("A Company", companies.First().Name);
    }

    [Fact]
    public async Task GetCompanyById_WhenCompanyExists_ReturnsCompany()
    {
        using var context = CreateDbContext();

        var company = new Company
        {
            Name = "AspenView",
            Location = "Remote"
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var controller = new CompaniesController(context);

        var result = await controller.GetCompanyById(company.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCompany = Assert.IsType<Company>(okResult.Value);

        Assert.Equal("AspenView", returnedCompany.Name);
    }

    [Fact]
    public async Task GetCompanyById_WhenCompanyDoesNotExist_ReturnsNotFound()
    {
        using var context = CreateDbContext();

        var controller = new CompaniesController(context);

        var result = await controller.GetCompanyById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateCompany_CreatesCompanyAndReturnsCreatedResult()
    {
        using var context = CreateDbContext();

        var controller = new CompaniesController(context);

        var request = new CreateCompanyRequest
        {
            Name = "TechNova Solutions",
            Website = "https://www.technova.com",
            Location = "Remote"
        };

        var result = await controller.CreateCompany(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var company = Assert.IsType<Company>(createdResult.Value);

        Assert.Equal("TechNova Solutions", company.Name);
        Assert.Equal(1, context.Companies.Count());
    }
}