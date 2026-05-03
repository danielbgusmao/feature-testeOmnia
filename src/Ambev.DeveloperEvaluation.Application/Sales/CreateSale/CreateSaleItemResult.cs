namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents the response for a single item created in a sale.
/// </summary>
/// <remarks>
/// This response contains detailed information about the item including
/// pricing, discount information, and total amount.
/// </remarks>
public class CreateSaleItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale item.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product in this sale item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied based on quantity.
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Gets or sets the total discount amount applied to this item.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the total amount for this item (Quantity * UnitPrice - DiscountAmount).
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }
}
