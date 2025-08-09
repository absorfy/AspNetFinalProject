namespace AspNetFinalProject.Services.Interfaces;

public interface INotificationService
{
    Task CreateForRecipientsAsync(Guid userActionLogId, IEnumerable<string> userProfileIds);
}