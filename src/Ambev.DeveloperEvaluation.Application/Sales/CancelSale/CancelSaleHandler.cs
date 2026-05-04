using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.ORM;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing CancelSaleCommand requests
/// </summary>
public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly DefaultContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<CancelSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of CancelSaleHandler
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="logger">The logger instance</param>
    public CancelSaleHandler(DefaultContext context, IMapper mapper, ILogger<CancelSaleHandler> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the CancelSaleCommand request
    /// </summary>
    /// <param name="request">The CancelSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cancelled sale details</returns>
    /// <exception cref="DomainException">
    /// Thrown when the sale is not found
    /// </exception>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        var sale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (sale == null)
            throw new DomainException($"Sale with ID {request.Id} not found");

        sale.Cancel();

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Event published: SaleCancelled | SaleId: {SaleId}", sale.Id);

        return _mapper.Map<CancelSaleResult>(sale);
    }
}
