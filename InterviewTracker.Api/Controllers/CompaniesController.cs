using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InterviewTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Company>>> GetCompanies(
    [FromQuery] PaginationRequest request)
    {
        var companies = await _companyService.GetCompaniesAsync(request);

        return Ok(companies);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Company>> GetCompanyById(int id)
    {
        var company = await _companyService.GetCompanyByIdAsync(id);

        if (company is null)
            return NotFound();

        return Ok(company);
    }

    [HttpPost]
    public async Task<ActionResult<Company>> CreateCompany(CreateCompanyRequest request)
    {
        var company = await _companyService.CreateCompanyAsync(request);

        return CreatedAtAction(nameof(GetCompanyById), new { id = company.Id }, company);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateCompany(int id, UpdateCompanyRequest request)
    {
        var result = await _companyService.UpdateCompanyAsync(id, request);

        if (!result.Success)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}