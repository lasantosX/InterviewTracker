using InterviewTracker.Business.Dtos;
using InterviewTracker.Business.Interfaces;
using InterviewTracker.Data.Interfaces;
using InterviewTracker.Domain.Models;
using InterviewTracker.Domain.Constants;

namespace InterviewTracker.Business.Services;

public class InterviewService : IInterviewService
{
    private readonly IInterviewRepository _interviewRepository;

    public InterviewService(IInterviewRepository interviewRepository)
    {
        _interviewRepository = interviewRepository;
    }

    public async Task<IEnumerable<Interview>> GetInterviewsAsync()
    {
        return await _interviewRepository.GetAllAsync();
    }

    public async Task<Interview?> GetInterviewByIdAsync(int id)
    {
        return await _interviewRepository.GetByIdAsync(id);
    }

    public async Task<(bool Success, string? ErrorMessage, Interview? Interview)> CreateInterviewAsync(CreateInterviewRequest request)
    {
        var companyExists = await _interviewRepository.CompanyExistsAsync(request.CompanyId);

        if (!InterviewStatuses.All.Contains(request.Status))
            return (false, "Invalid interview status.", null);

        if (!companyExists)
            return (false, "Company does not exist.", null);

        if (request.RecruiterId.HasValue)
        {
            var recruiterExists = await _interviewRepository.RecruiterExistsAsync(request.RecruiterId.Value);

            if (!recruiterExists)
                return (false, "Recruiter does not exist.", null);
        }

        var interview = new Interview
        {
            RoleTitle = request.RoleTitle,
            Status = request.Status,
            InterviewDate = request.InterviewDate,
            Notes = request.Notes,
            ExpectedSalary = request.ExpectedSalary,
            CompanyId = request.CompanyId,
            RecruiterId = request.RecruiterId,
            CreatedAt = DateTime.UtcNow
        };

        var createdInterview = await _interviewRepository.AddAsync(interview);

        return (true, null, createdInterview);
    }
}