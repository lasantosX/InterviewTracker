namespace InterviewTracker.Business.Dtos;

public class CreateInterviewRequest
{
    public string RoleTitle { get; set; } = string.Empty;

    public string Status { get; set; } = "Applied";

    public DateTime? InterviewDate { get; set; }

    public string? Notes { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public int CompanyId { get; set; }

    public int? RecruiterId { get; set; }
}