using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace InterviewTracker.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InterviewsController : ControllerBase
{
    private readonly IInterviewService _interviewService;

    public InterviewsController(IInterviewService interviewService)
    {
        _interviewService = interviewService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<Interview>>> GetInterviews(
    [FromQuery] InterviewFilterRequest request)
    {
        var interviews = await _interviewService.GetInterviewsAsync(request);

        return Ok(interviews);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Interview>> GetInterviewById(int id)
    {
        var interview = await _interviewService.GetInterviewByIdAsync(id);

        if (interview is null)
            return NotFound();

        return Ok(interview);
    }

    [HttpPost]
    public async Task<ActionResult<Interview>> CreateInterview(CreateInterviewRequest request)
    {
        var result = await _interviewService.CreateInterviewAsync(request);

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return CreatedAtAction(
            nameof(GetInterviewById),
            new { id = result.Interview!.Id },
            result.Interview);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateInterview(int id, UpdateInterviewRequest request)
    {
        var result = await _interviewService.UpdateInterviewAsync(id, request);

        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteInterview(int id)
    {
        var result = await _interviewService.DeleteInterviewAsync(id);

        if (!result.Success)
            return NotFound(result.ErrorMessage);

        return NoContent();
    }
}