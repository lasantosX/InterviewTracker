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

    public async Task<IEnumerable<Recruiter>> GetRecruitersAsync()
    {
        return await _recruiterRepository.GetAllAsync();
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
}