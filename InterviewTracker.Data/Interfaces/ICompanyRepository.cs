using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data.Interfaces;

public interface ICompanyRepository
{
    Task<(IEnumerable<Company> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
    Task<Company?> GetByIdAsync(int id);
    Task<Company> AddAsync(Company company);
    Task<bool> UpdateAsync(Company company);
    Task<bool> DeleteAsync(Company company);
    Task<bool> HasInterviewsAsync(int companyId);
}