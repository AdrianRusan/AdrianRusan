namespace PugPlatform.Domain.Entities;

public class ParcelTaxStatus
{
    public Guid Id { get; set; }
    public Guid ParcelId { get; set; }
    public int LastPaidYear { get; set; }
    public int YearsOverdue { get; set; }
    public decimal TotalDue { get; set; }
    public decimal TotalPaid { get; set; }
    public string Status { get; set; } = string.Empty;

    // Navigation properties
    public Parcel Parcel { get; set; } = null!;
}
