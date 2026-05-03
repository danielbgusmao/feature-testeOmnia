using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a single item in a sale.
/// Includes product information, quantity, pricing, and discount rules.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the sale this item belongs to.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the product (External Identity).
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the name of the product.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product in this sale item.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the discount percentage applied based on quantity.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    /// Gets the total discount amount applied to this item.
    /// </summary>
    public decimal DiscountAmount { get; private set; }

    /// <summary>
    /// Gets the total amount for this item (Quantity * UnitPrice - DiscountAmount).
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this item has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the date and time when this item was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to this item.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when this item was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// Protected constructor for Entity Framework Core.
    /// </summary>
    protected SaleItem()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class with the specified parameters.
    /// </summary>
    /// <param name="saleId">The unique identifier of the sale.</param>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="DomainException">
    /// Thrown when product ID is an empty GUID, product name is null/empty/whitespace,
    /// quantity is less than or equal to 0, unit price is less than or equal to 0,
    /// or quantity exceeds 20 units.
    /// </exception>
    public SaleItem(Guid saleId, Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product ID cannot be an empty GUID");

        if (string.IsNullOrWhiteSpace(productName))
            throw new DomainException("Product name cannot be null, empty, or whitespace");

        if (quantity <= 0)
            throw new DomainException("Item quantity must be greater than 0");

        if (unitPrice <= 0)
            throw new DomainException("Unit price must be greater than 0");

        if (quantity > 20)
            throw new DomainException("Item quantity cannot exceed 20 units");

        SaleId = saleId;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        CreatedAt = DateTime.UtcNow;
        IsCancelled = false;

        CalculateDiscount();
        CalculateTotalAmount();
    }

    /// <summary>
    /// Updates the quantity and unit price of this item.
    /// </summary>
    /// <param name="quantity">The new quantity.</param>
    /// <param name="unitPrice">The new unit price.</param>
    /// <exception cref="DomainException">
    /// Thrown when the item is cancelled, quantity is less than or equal to 0,
    /// unit price is less than or equal to 0, or quantity exceeds 20 units.
    /// </exception>
    public void Update(int quantity, decimal unitPrice)
    {
        if (IsCancelled)
            throw new DomainException("Cannot update a cancelled item");

        if (quantity <= 0)
            throw new DomainException("Item quantity must be greater than 0");

        if (unitPrice <= 0)
            throw new DomainException("Unit price must be greater than 0");

        if (quantity > 20)
            throw new DomainException("Item quantity cannot exceed 20 units");

        Quantity = quantity;
        UnitPrice = unitPrice;
        UpdatedAt = DateTime.UtcNow;

        CalculateDiscount();
        CalculateTotalAmount();
    }

    /// <summary>
    /// Cancels this item.
    /// </summary>
    /// <exception cref="DomainException">Thrown when the item is already cancelled.</exception>
    public void Cancel()
    {
        if (IsCancelled)
            throw new DomainException("Item is already cancelled");

        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Calculates the discount percentage based on the quantity.
    /// </summary>
    /// <remarks>
    /// Discount rules:
    /// - 1 to 3 units: 0% discount
    /// - 4 to 9 units: 10% discount
    /// - 10 to 20 units: 20% discount
    /// </remarks>
    private void CalculateDiscount()
    {
        DiscountPercentage = Quantity switch
        {
            >= 1 and <= 3 => 0m,
            >= 4 and <= 9 => 0.10m,
            >= 10 and <= 20 => 0.20m,
            _ => 0m
        };

        DiscountAmount = Quantity * UnitPrice * DiscountPercentage;
    }

    /// <summary>
    /// Calculates the total amount for this item.
    /// </summary>
    private void CalculateTotalAmount()
    {
        TotalAmount = (Quantity * UnitPrice) - DiscountAmount;
    }
}
