using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(int id);
    Task<Company> AddAsync(Company company);
}