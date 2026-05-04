namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Request model for ListSales operation with pagination and filtering options.
/// </summary>
public class ListSalesRequest
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
    /// Gets or sets the optional start date filter (inclusive) in format YYYY-MM-DD.
    /// </summary>
    public string? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the optional end date filter (inclusive) in format YYYY-MM-DD.
    /// </summary>
    public string? EndDate { get; set; }

    /// <summary>
    /// Gets or sets the field to order by (saleDate, saleNumber, totalAmount, customerName, branchName).
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets the order direction (asc or desc).
    /// </summary>
    public string? OrderDirection { get; set; }
}
