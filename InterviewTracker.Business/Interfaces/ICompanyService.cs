using InterviewTracker.Business.Dtos;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetCompaniesAsync();

    Task<Company?> GetCompanyByIdAsync(int id);

    Task<Company> CreateCompanyAsync(CreateCompanyRequest request);
}