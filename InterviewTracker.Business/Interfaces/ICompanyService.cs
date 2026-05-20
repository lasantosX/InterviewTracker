using InterviewTracker.Business.Dtos;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface ICompanyService
{
    Task<PagedResult<Company>> GetCompaniesAsync(PaginationRequest request);

    Task<Company?> GetCompanyByIdAsync(int id);

    Task<Company> CreateCompanyAsync(CreateCompanyRequest request);

    Task<(bool Success, string? ErrorMessage)> UpdateCompanyAsync(int id, UpdateCompanyRequest request);
}