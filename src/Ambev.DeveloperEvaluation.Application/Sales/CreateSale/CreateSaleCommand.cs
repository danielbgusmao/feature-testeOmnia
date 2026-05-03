using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Command for creating a new sale with multiple items.
/// </summary>
/// <remarks>
/// This command is used to capture the required data for creating a sale,
/// including customer information, branch information, and sale items.
/// It implements <see cref="IRequest{TResponse}"/> to initiate the request
/// that returns a <see cref="CreateSaleResult"/>.
/// </remarks>
public class CreateSaleCommand : IRequest<CreateSaleResult>
{
    /// <summary>
    /// Gets or sets the sale number.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the customer.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the customer.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the branch.
    /// </summary>
    public Guid BranchId { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch.
    /// </summary>
    public string BranchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the collection of items for this sale.
    /// </summary>
    public List<CreateSaleItemCommand> Items { get; set; } = new List<CreateSaleItemCommand>();
}
