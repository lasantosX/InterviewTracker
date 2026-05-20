namespace InterviewTracker.Domain.Constants;

public static class InterviewStatuses
{
    public const string Applied = "Applied";
    public const string RecruiterScreen = "Recruiter Screen";
    public const string TechnicalInterview = "Technical Interview";
    public const string Offer = "Offer";
    public const string Rejected = "Rejected";

    public static readonly string[] All =
    {
        Applied,
        RecruiterScreen,
        TechnicalInterview,
        Offer,
        Rejected
    };
}