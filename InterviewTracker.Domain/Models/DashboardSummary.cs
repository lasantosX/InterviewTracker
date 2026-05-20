namespace InterviewTracker.Domain.Models;

public class DashboardSummary
{
    public int TotalCompanies { get; set; }
    public int TotalRecruiters { get; set; }
    public int TotalInterviews { get; set; }
    public int AppliedCount { get; set; }
    public int TechnicalInterviewCount { get; set; }
    public int OfferCount { get; set; }
    public int RejectedCount { get; set; }
    public decimal AverageExpectedSalary { get; set; }
}