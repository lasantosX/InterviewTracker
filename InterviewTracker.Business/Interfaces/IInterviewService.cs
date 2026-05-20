using InterviewTracker.Business.Dtos;
using InterviewTracker.Domain.Models;

namespace InterviewTracker.Business.Interfaces;

public interface IInterviewService
{
    Task<PagedResult<Interview>> GetInterviewsAsync(InterviewFilterRequest request);

    Task<Interview?> GetInterviewByIdAsync(int id);

    Task<(bool Success, string? ErrorMessage, Interview? Interview)> CreateInterviewAsync(CreateInterviewRequest request);

    Task<(bool Success, string? ErrorMessage)> UpdateInterviewAsync(int id, UpdateInterviewRequest request);

    Task<(bool Success, string? ErrorMessage)> DeleteInterviewAsync(int id);
}