using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Business.Dtos;

public class CreateCompanyRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Website { get; set; }

    [MaxLength(150)]
    public string? Location { get; set; }
}