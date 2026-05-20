namespace InterviewTracker.Business.Dtos;

public class CreateRecruiterRequest
{
    public string FullName { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? LinkedInUrl { get; set; }
}