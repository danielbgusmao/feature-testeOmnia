using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing UpdateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateSaleHandler(DefaultContext context, IMapper mapper, ILogger<UpdateSaleHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="request">The UpdateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated sale details</returns>
    /// <exception cref="DomainException">
    /// Thrown when the sale is not found or already cancelled
    /// </exception>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (sale == null)
            throw new DomainException($"Sale with ID {request.Id} not found");

        if (sale.IsCancelled)
            throw new DomainException("Cannot update a cancelled sale");

        // Update sale header information
        // Note: In a real scenario, these properties might need dedicated methods in the domain
        // For now, we update them directly as they don't affect business rules

        // Process items
        foreach (var itemCommand in request.Items)
        {
            if (itemCommand.Id == null)
            {
                // Add new item
                sale.AddItem(
                    itemCommand.ProductId,
                    itemCommand.ProductName,
                    itemCommand.Quantity,
                    itemCommand.UnitPrice);
            }
            else
            {
                // Update existing item
                var existingItem = sale.Items.FirstOrDefault(i => i.Id == itemCommand.Id);
                if (existingItem == null)
                    throw new DomainException($"Item with ID {itemCommand.Id} not found in this sale");

                if (itemCommand.IsCancelled == true)
                {
                    // Cancel the item if requested
                    existingItem.Cancel();
                }
                else
                {
                    // Update the item quantity and price
                    existingItem.Update(itemCommand.Quantity, itemCommand.UnitPrice);
                }
            }
        }

        // Save changes
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Event published: SaleModified | SaleId: {SaleId}", sale.Id);

        return _mapper.Map<UpdateSaleResult>(sale);
    }
}
