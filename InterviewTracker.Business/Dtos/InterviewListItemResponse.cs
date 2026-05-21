namespace InterviewTracker.Business.Dtos;

public class InterviewListItemResponse
{
    public int Id { get; set; }
    public string RoleTitle { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? InterviewDate { get; set; }
    public string? Notes { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public int CompanyId { get; set; }
    public string? CompanyName { get; set; }
    public int? RecruiterId { get; set; }
    public string? RecruiterName { get; set; }
    public DateTime CreatedAt { get; set; }
}