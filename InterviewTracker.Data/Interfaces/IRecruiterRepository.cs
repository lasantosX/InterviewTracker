using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IRecruiterRepository
{
    Task<(IEnumerable<Recruiter> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);

    Task<Recruiter?> GetByIdAsync(int id);

    Task<Recruiter> AddAsync(Recruiter recruiter);

    Task<bool> UpdateAsync(Recruiter recruiter);
}