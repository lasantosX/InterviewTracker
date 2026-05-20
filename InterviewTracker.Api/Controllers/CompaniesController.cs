using InterviewTracker.Api.Dtos;
using InterviewTracker.Data;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterviewTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly InterviewTrackerDbContext _context;

    public CompaniesController(InterviewTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
    {
        var companies = await _context.Companies
            .OrderBy(x => x.Name)
            .ToListAsync();

        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Company>> GetCompanyById(int id)
    {
        var company = await _context.Companies.FindAsync(id);

        if (company is null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<Company>> CreateCompany(CreateCompanyRequest request)
    {
        var company = new Company
        {
            Name = request.Name,
            Website = request.Website,
            Location = request.Location,
            CreatedAt = DateTime.UtcNow
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
    }
}