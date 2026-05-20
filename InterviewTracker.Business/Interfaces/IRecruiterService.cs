using InterviewTracker.Business.Dtos;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface IRecruiterService
{
    Task<PagedResult<Recruiter>> GetRecruitersAsync(PaginationRequest request);

    Task<Recruiter?> GetRecruiterByIdAsync(int id);

    Task<Recruiter> CreateRecruiterAsync(CreateRecruiterRequest request);

    Task<(bool Success, string? ErrorMessage)> UpdateRecruiterAsync(int id, UpdateRecruiterRequest request);

    Task<(bool Success, string? ErrorMessage)> DeleteRecruiterAsync(int id);
}