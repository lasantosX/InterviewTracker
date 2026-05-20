using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface ICompanyRepository
{
    Task<(IEnumerable<Company> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<Company?> GetByIdAsync(int id);
    Task<Company> AddAsync(Company company);
}