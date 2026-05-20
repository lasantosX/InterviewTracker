using System.ComponentModel.DataAnnotations;

namespace InterviewTracker.Business.Dtos;

public class InterviewFilterRequest : PaginationRequest
{
    [MaxLength(50)]
    public string? Status { get; set; }

    public int? CompanyId { get; set; }

    public int? RecruiterId { get; set; }
}