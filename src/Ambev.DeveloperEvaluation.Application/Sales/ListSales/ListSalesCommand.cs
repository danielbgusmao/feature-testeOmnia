using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for listing sales with pagination and filtering options.
/// </summary>
public class ListSalesCommand : IRequest<ListSalesResult>
{
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the optional sale number filter (partial match).
    /// </summary>
    public string? SaleNumber { get; set; }

    /// <summary>
    /// Gets or sets the optional customer ID filter (exact match).
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the optional customer name filter (partial match).
    /// </summary>
    public string? CustomerName { get; set; }

    /// <summary>
    /// Gets or sets the optional branch ID filter (exact match).
    /// </summary>
    public Guid? BranchId { get; set; }

    /// <summary>
    /// Gets or sets the optional branch name filter (partial match).
    /// </summary>
    public string? BranchName { get; set; }

    /// <summary>
    /// Gets or sets the optional filter for cancelled sales.
    /// </summary>
    public bool? IsCancelled { get; set; }

    /// <summary>
    /// Gets or sets the optional start date filter (inclusive).
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the optional end date filter (inclusive).
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the field to order by (saleDate, saleNumber, totalAmount, customerName, branchName).
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the order direction (asc or desc).
    /// </summary>
    public string? OrderDirection { get; set; }
}
