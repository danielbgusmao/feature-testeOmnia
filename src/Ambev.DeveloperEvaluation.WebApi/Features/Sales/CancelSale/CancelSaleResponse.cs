namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// API response model for CancelSale operation
/// </summary>
public class CancelSaleResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the cancelled sale.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when this sale was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; set; }
}
