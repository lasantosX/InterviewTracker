using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface IInterviewRepository
{
    Task<IEnumerable<Interview>> GetAllAsync();
    Task<Interview?> GetByIdAsync(int id);
    Task<Interview> AddAsync(Interview interview);
    Task<bool> CompanyExistsAsync(int companyId);
    Task<bool> RecruiterExistsAsync(int recruiterId);
}