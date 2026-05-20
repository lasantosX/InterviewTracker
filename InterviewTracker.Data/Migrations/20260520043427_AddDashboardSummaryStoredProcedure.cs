using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterviewTracker.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDashboardSummaryStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE dbo.GetInterviewDashboardSummary
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
                        ISNULL(AVG(CAST(ExpectedSalary AS DECIMAL(18,2))), 0) AS AverageExpectedSalary
                    FROM Interviews;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP PROCEDURE IF EXISTS dbo.GetInterviewDashboardSummary;
            ");
        }
    }
}
