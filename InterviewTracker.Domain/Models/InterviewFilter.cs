namespace InterviewTracker.Domain.Models;

public class InterviewFilter
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Status { get; set; }

    public int? CompanyId { get; set; }

    public int? RecruiterId { get; set; }
}