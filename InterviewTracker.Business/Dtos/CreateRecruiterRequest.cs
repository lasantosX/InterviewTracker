using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Business.Dtos;

public class CreateRecruiterRequest
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [EmailAddress]
    [MaxLength(200)]
    public string? Email { get; set; }

    [MaxLength(300)]
    public string? LinkedInUrl { get; set; }
}