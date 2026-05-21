using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using InterviewTracker.Domain.Constants;

namespace InterviewTracker.Business.Services;

public class InterviewService : IInterviewService
{
    private readonly IInterviewRepository _interviewRepository;

    public InterviewService(IInterviewRepository interviewRepository)
    {
        _interviewRepository = interviewRepository;
    }

    public async Task<PagedResult<InterviewListItemResponse>> GetInterviewsAsync(InterviewFilterRequest request)
    {
        var filter = new InterviewFilter
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            Status = request.Status,
            CompanyId = request.CompanyId,
            RecruiterId = request.RecruiterId
        };

        var (items, totalCount) = await _interviewRepository.GetFilteredAsync(filter);

        var responseItems = items.Select(x => new InterviewListItemResponse
        {
            Id = x.Id,
            RoleTitle = x.RoleTitle,
            Status = x.Status,
            InterviewDate = x.InterviewDate,
            Notes = x.Notes,
            ExpectedSalary = x.ExpectedSalary,
            CompanyId = x.CompanyId,
            CompanyName = x.Company?.Name,
            RecruiterId = x.RecruiterId,
            RecruiterName = x.Recruiter?.FullName,
            CreatedAt = x.CreatedAt
        });

        return new PagedResult<InterviewListItemResponse>
        {
            Items = responseItems,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Interview?> GetInterviewByIdAsync(int id)
    {
        return await _interviewRepository.GetByIdAsync(id);
    }

    public async Task<(bool Success, string? ErrorMessage, Interview? Interview)> CreateInterviewAsync(CreateInterviewRequest request)
    {
        var companyExists = await _interviewRepository.CompanyExistsAsync(request.CompanyId);

        if (!InterviewStatuses.All.Contains(request.Status))
            return (false, "Invalid interview status.", null);

        if (!companyExists)
            return (false, "Company does not exist.", null);

        if (request.RecruiterId.HasValue)
        {
            var recruiterExists = await _interviewRepository.RecruiterExistsAsync(request.RecruiterId.Value);

            if (!recruiterExists)
                return (false, "Recruiter does not exist.", null);
        }

        var interview = new Interview
        {
            RoleTitle = request.RoleTitle,
            Status = request.Status,
            InterviewDate = request.InterviewDate,
            Notes = request.Notes,
            ExpectedSalary = request.ExpectedSalary,
            CompanyId = request.CompanyId,
            RecruiterId = request.RecruiterId,
            CreatedAt = DateTime.UtcNow
        };

        var createdInterview = await _interviewRepository.AddAsync(interview);

        return (true, null, createdInterview);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateInterviewAsync(int id, UpdateInterviewRequest request)
    {
        if (!InterviewStatuses.All.Contains(request.Status))
            return (false, "Invalid interview status.");

        var interview = await _interviewRepository.GetByIdAsync(id);

        if (interview is null)
            return (false, "Interview does not exist.");

        var companyExists = await _interviewRepository.CompanyExistsAsync(request.CompanyId);

        if (!companyExists)
            return (false, "Company does not exist.");

        if (request.RecruiterId.HasValue)
        {
            var recruiterExists = await _interviewRepository.RecruiterExistsAsync(request.RecruiterId.Value);

            if (!recruiterExists)
                return (false, "Recruiter does not exist.");
        }

        interview.RoleTitle = request.RoleTitle;
        interview.Status = request.Status;
        interview.InterviewDate = request.InterviewDate;
        interview.Notes = request.Notes;
        interview.ExpectedSalary = request.ExpectedSalary;
        interview.CompanyId = request.CompanyId;
        interview.RecruiterId = request.RecruiterId;

        await _interviewRepository.UpdateAsync(interview);

        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteInterviewAsync(int id)
    {
        var interview = await _interviewRepository.GetByIdAsync(id);

        if (interview is null)
            return (false, "Interview does not exist.");

        await _interviewRepository.DeleteAsync(interview);

        return (true, null);
    }
}