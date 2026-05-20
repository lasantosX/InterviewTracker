using InterviewTracker.Business.Dtos;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface IRecruiterService
{
    Task<IEnumerable<Recruiter>> GetRecruitersAsync();

    Task<Recruiter?> GetRecruiterByIdAsync(int id);

    Task<Recruiter> CreateRecruiterAsync(CreateRecruiterRequest request);
}