namespace PugPlatform.Application.Interfaces;

public interface IEmailService
{
    Task SendApplicationConfirmationAsync(
        string email,
        string firstName,
        string trackingNumber,
        string language = "ro",
        CancellationToken cancellationToken = default);

    Task SendApplicationStatusChangeAsync(
        string email,
        string firstName,
        string trackingNumber,
        string newStatus,
        string? comment,
        string language = "ro",
        CancellationToken cancellationToken = default);

    Task SendIssueConfirmationAsync(
        string email,
        string firstName,
        Guid issueId,
        string language = "ro",
        CancellationToken cancellationToken = default);

    Task SendIssueResponseAsync(
        string email,
        string firstName,
        Guid issueId,
        string response,
        string language = "ro",
        CancellationToken cancellationToken = default);

    Task SendSaleRenewalReminderAsync(
        string email,
        string firstName,
        Guid saleId,
        DateTime expiresAt,
        string language = "ro",
        CancellationToken cancellationToken = default);

    Task SendScanCompletedAsync(
        string email,
        string firstName,
        Guid scanId,
        string language = "ro",
        CancellationToken cancellationToken = default);
}
