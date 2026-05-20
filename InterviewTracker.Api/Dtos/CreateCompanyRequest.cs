namespace InterviewTracker.Api.Dtos;

public class CreateCompanyRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Website { get; set; }
    public string? Location { get; set; }
}