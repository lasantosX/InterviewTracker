using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IInterviewRepository
{
    Task<(IEnumerable<Interview> Items, int TotalCount)> GetFilteredAsync(InterviewFilter filter);
    Task<Interview?> GetByIdAsync(int id);
    Task<Interview> AddAsync(Interview interview);
    Task<bool> CompanyExistsAsync(int companyId);
    Task<bool> RecruiterExistsAsync(int recruiterId);
    Task<bool> UpdateAsync(Interview interview);
    Task<bool> DeleteAsync(Interview interview);
}