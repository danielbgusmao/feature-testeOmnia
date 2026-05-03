namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Represents a single item to be added in a sale creation command.
/// </summary>
/// <remarks>
/// This class contains the product information and quantity details required to create a sale item.
/// </remarks>
public class CreateSaleItemCommand
{
    /// <summary>
    /// Gets or sets the unique identifier of the product (External Identity).
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
}
