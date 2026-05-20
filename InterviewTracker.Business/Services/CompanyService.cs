using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Services;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<PagedResult<Company>> GetCompaniesAsync(PaginationRequest request)
    {
        var (items, totalCount) = await _companyRepository.GetPagedAsync(
            request.PageNumber,
            request.PageSize);

        return new PagedResult<Company>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Company?> GetCompanyByIdAsync(int id)
    {
        return await _companyRepository.GetByIdAsync(id);
    }

    public async Task<Company> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var company = new Company
        {
            Name = request.Name,
            Website = request.Website,
            Location = request.Location,
            CreatedAt = DateTime.UtcNow
        };

        return await _companyRepository.AddAsync(company);
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateCompanyAsync(int id, UpdateCompanyRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company is null)
            return (false, "Company does not exist.");

        company.Name = request.Name;
        company.Website = request.Website;
        company.Location = request.Location;

        await _companyRepository.UpdateAsync(company);

        return (true, null);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteCompanyAsync(int id)
    {
        var company = await _companyRepository.GetByIdAsync(id);

        if (company is null)
            return (false, "Company does not exist.");

        var hasInterviews = await _companyRepository.HasInterviewsAsync(id);

        if (hasInterviews)
            return (false, "Company cannot be deleted because it has related interviews.");

        await _companyRepository.DeleteAsync(company);

        return (true, null);
    }
}