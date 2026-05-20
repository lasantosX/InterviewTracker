using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Services;

public class RecruiterService : IRecruiterService
{
    private readonly IRecruiterRepository _recruiterRepository;

    public RecruiterService(IRecruiterRepository recruiterRepository)
    {
        _recruiterRepository = recruiterRepository;
    }

    public async Task<PagedResult<Recruiter>> GetRecruitersAsync(PaginationRequest request)
    {
        var (items, totalCount) = await _recruiterRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize);

        return new PagedResult<Recruiter>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Recruiter?> GetRecruiterByIdAsync(int id)
    {
        return await _recruiterRepository.GetByIdAsync(id);
    }

    public async Task<Recruiter> CreateRecruiterAsync(CreateRecruiterRequest request)
    {
        var recruiter = new Recruiter
        {
            FullName = request.FullName,
            Email = request.Email,
            LinkedInUrl = request.LinkedInUrl,
            CreatedAt = DateTime.UtcNow
        };

        return await _recruiterRepository.AddAsync(recruiter);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateRecruiterAsync(int id, UpdateRecruiterRequest request)
    {
        var recruiter = await _recruiterRepository.GetByIdAsync(id);

        if (recruiter is null)
            return (false, "Recruiter does not exist.");

        recruiter.FullName = request.FullName;
        recruiter.Email = request.Email;
        recruiter.LinkedInUrl = request.LinkedInUrl;

        await _recruiterRepository.UpdateAsync(recruiter);

        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteRecruiterAsync(int id)
    {
        var recruiter = await _recruiterRepository.GetByIdAsync(id);

        if (recruiter is null)
            return (false, "Recruiter does not exist.");

        await _recruiterRepository.DeleteAsync(recruiter);

        return (true, null);
    }
}