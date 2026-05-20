using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IRecruiterRepository
{
    Task<IEnumerable<Recruiter>> GetAllAsync();

    Task<Recruiter?> GetByIdAsync(int id);

    Task<Recruiter> AddAsync(Recruiter recruiter);
}