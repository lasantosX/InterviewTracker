using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Business.Dtos;

public class UpdateInterviewRequest
{
    [Required]
    [MaxLength(200)]
    public string RoleTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Applied";

    public DateTime? InterviewDate { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }

    [Range(1, 999999)]
    public decimal? ExpectedSalary { get; set; }

    [Range(1, int.MaxValue)]
    public int CompanyId { get; set; }

    public int? RecruiterId { get; set; }
}