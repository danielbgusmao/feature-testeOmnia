using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a sale with multiple items, customer and branch information.
/// Handles business rules for adding, updating, and cancelling items.
/// </summary>
public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the sale number.
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date of the sale.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the customer (External Identity).
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the name of the customer.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the branch (External Identity).
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the name of the branch.
    /// </summary>
    public string BranchName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the total amount of the sale (considering only non-cancelled items).
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this sale has been cancelled.
    /// </summary>
    public bool IsCancelled { get; private set; }

    /// <summary>
    /// Gets the collection of items in this sale.
    /// </summary>
    public ICollection<SaleItem> Items { get; private set; } = new List<SaleItem>();

    /// <summary>
    /// Gets the date and time when this sale was created.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time of the last update to this sale.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }

    /// <summary>
    /// Gets the date and time when this sale was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; private set; }

    /// <summary>
    /// Initializes a new instance of the Sale class.
    /// Protected constructor for Entity Framework Core.
    /// </summary>
    protected Sale()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Initializes a new instance of the Sale class with the specified parameters.
    /// </summary>
    /// <param name="saleNumber">The sale number.</param>
    /// <param name="customerId">The unique identifier of the customer.</param>
    /// <param name="customerName">The name of the customer.</param>
    /// <param name="branchId">The unique identifier of the branch.</param>
    /// <param name="branchName">The name of the branch.</param>
    /// <exception cref="DomainException">
    /// Thrown when sale number, customer name, or branch name are null, empty, or whitespace,
    /// or when customer ID or branch ID are empty GUIDs.
    /// </exception>
    public Sale(
        string saleNumber,
        Guid customerId,
        string customerName,
        Guid branchId,
        string branchName)
    {
        if (string.IsNullOrWhiteSpace(saleNumber))
            throw new DomainException("Sale number cannot be null, empty, or whitespace");

        if (customerId == Guid.Empty)
            throw new DomainException("Customer ID cannot be an empty GUID");

        if (string.IsNullOrWhiteSpace(customerName))
            throw new DomainException("Customer name cannot be null, empty, or whitespace");

        if (branchId == Guid.Empty)
            throw new DomainException("Branch ID cannot be an empty GUID");

        if (string.IsNullOrWhiteSpace(branchName))
            throw new DomainException("Branch name cannot be null, empty, or whitespace");

        SaleNumber = saleNumber;
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        CreatedAt = DateTime.UtcNow;
        IsCancelled = false;
        TotalAmount = 0;
    }

    /// <summary>
    /// Adds a new item to the sale.
    /// </summary>
    /// <param name="productId">The unique identifier of the product.</param>
    /// <param name="productName">The name of the product.</param>
    /// <param name="quantity">The quantity of the product.</param>
    /// <param name="unitPrice">The unit price of the product.</param>
    /// <exception cref="DomainException">
    /// Thrown when the sale is cancelled, quantity is invalid, unit price is invalid,
    /// or quantity exceeds 20 units.
    /// </exception>
    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (IsCancelled)
            throw new DomainException("Cannot add items to a cancelled sale");

        var existingItem = Items.FirstOrDefault(i =>
            i.ProductId == productId &&
            !i.IsCancelled);

        if (existingItem is not null)
        {
            existingItem.Update(existingItem.Quantity + quantity, unitPrice);
        }
        else
        {
            var item = new SaleItem(Id, productId, productName, quantity, unitPrice);
            Items.Add(item);
        }

        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
    }

    /// <summary>
    /// Updates an existing item in the sale.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to update.</param>
    /// <param name="quantity">The new quantity.</param>
    /// <param name="unitPrice">The new unit price.</param>
    /// <exception cref="DomainException">
    /// Thrown when the sale is cancelled or the item is not found.
    /// </exception>
    public void UpdateItem(Guid itemId, int quantity, decimal unitPrice)
    {
        if (IsCancelled)
            throw new DomainException("Cannot update items in a cancelled sale");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new DomainException("Item not found in this sale");

        item.Update(quantity, unitPrice);
        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
    }

    /// <summary>
    /// Cancels an item in the sale.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to cancel.</param>
    /// <exception cref="DomainException">
    /// Thrown when the sale is cancelled or the item is not found.
    /// </exception>
    public void CancelItem(Guid itemId)
    {
        if (IsCancelled)
            throw new DomainException("Cannot cancel items in a cancelled sale");

        var item = Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null)
            throw new DomainException("Item not found in this sale");

        item.Cancel();
        UpdatedAt = DateTime.UtcNow;
        RecalculateTotal();
    }

    /// <summary>
    /// Cancels the entire sale.
    /// </summary>
    public void Cancel()
    {
        if (IsCancelled)
            throw new DomainException("Sale is already cancelled");

        foreach (var item in Items.Where(i => !i.IsCancelled))
        {
            item.Cancel();
        }

        IsCancelled = true;
        CancelledAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        //Para manter o histórico de itens cancelados, não recalculamos o total para zero, mas sim somamos apenas os itens não cancelados. Assim, o total reflete o valor dos itens que foram efetivamente vendidos, mesmo que a venda como um todo esteja cancelada.
        //RecalculateTotal();
    }

    /// <summary>
    /// Recalculates the total amount of the sale based on non-cancelled items.
    /// </summary>
    public void RecalculateTotal()
    {
        TotalAmount = Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.TotalAmount);
    }
}
