CREATE PROCEDURE dbo.GetInterviewDashboardSummary
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        (SELECT COUNT(*) FROM Companies) AS TotalCompanies,
        (SELECT COUNT(*) FROM Recruiters) AS TotalRecruiters,
        (SELECT COUNT(*) FROM Interviews) AS TotalInterviews,
        (SELECT COUNT(*) FROM Interviews WHERE Status = 'Applied') AS AppliedCount,
        (SELECT COUNT(*) FROM Interviews WHERE Status = 'Technical Interview') AS TechnicalInterviewCount,
        (SELECT COUNT(*) FROM Interviews WHERE Status = 'Offer') AS OfferCount,
        (SELECT COUNT(*) FROM Interviews WHERE Status = 'Rejected') AS RejectedCount,
        ISNULL(AVG(CAST(ExpectedSalary AS DECIMAL(18,2))), 0) AS AverageExpectedSalary;
END
GO