using DatingApp.Domain.Administrators;

namespace DatingApp.Domain.Users;

public class UserFlag
{
    public string Id { get; }
    public string? ReportedById { get; }
    public string? ReporterComment { get; }
    public DateTime? ReportedAt { get; }
    public string? Description { get; private set; }
    public FlagReason Reason { get; private set; }
    public FlagStatus Status { get; private set; }
    public bool IsReviewed { get; private set; }
    public string? ReviewedById { get; private set; }
    public DateTime ReviewedAt { get; private set; }
    public string? ReviewerComment { get; private set; }
    public virtual Administrator? ReviewedBy { get; }
    public virtual User? ReportedBy { get; }

    private UserFlag() 
    {
        Id = default!;
    }

    public UserFlag(string reprotedBy, string comment, DateTime reportedAt, FlagReason reason)
    {
        Id = default!;
        ReportedById = reprotedBy;
        ReporterComment = comment;
        ReportedAt = reportedAt;
        Reason = reason;
        Status = FlagStatus.Active;
        IsReviewed = false;
    }

    public void Review(string reviewerId, string comment, DateTime reviewedAt, FlagStatus status)
    {
        ReviewedById = reviewerId;
        ReviewerComment = comment;
        ReviewedAt = reviewedAt;
        Status = status;
        IsReviewed = true;
    }
}