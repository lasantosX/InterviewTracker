namespace InterviewTracker.Domain.Models;

public class Interview
{
    public int Id { get; set; }

    public string RoleTitle { get; set; } = string.Empty;

    public string Status { get; set; } = "Applied";

    public DateTime? InterviewDate { get; set; }

    public string? Notes { get; set; }

    public decimal? ExpectedSalary { get; set; }

    public int CompanyId { get; set; }

    public Company? Company { get; set; }

    public int? RecruiterId { get; set; }

    public Recruiter? Recruiter { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}