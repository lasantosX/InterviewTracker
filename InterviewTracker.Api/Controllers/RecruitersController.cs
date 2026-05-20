using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InterviewTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecruitersController : ControllerBase
{
    private readonly IRecruiterService _recruiterService;

    public RecruitersController(IRecruiterService recruiterService)
    {
        _recruiterService = recruiterService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Recruiter>>> GetRecruiters(
        [FromQuery] PaginationRequest request)
    {
        var recruiters = await _recruiterService.GetRecruitersAsync(request);

        return Ok(recruiters);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Recruiter>> GetRecruiterById(int id)
    {
        var recruiter = await _recruiterService.GetRecruiterByIdAsync(id);

        if (recruiter is null)
            return NotFound();

        return Ok(recruiter);
    }

    [HttpPost]
    public async Task<ActionResult<Recruiter>> CreateRecruiter(CreateRecruiterRequest request)
    {
        var recruiter = await _recruiterService.CreateRecruiterAsync(request);

        return CreatedAtAction(nameof(GetRecruiterById), new { id = recruiter.Id }, recruiter);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateRecruiter(int id, UpdateRecruiterRequest request)
    {
        var result = await _recruiterService.UpdateRecruiterAsync(id, request);

        if (!result.Success)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}