using Ambev.DeveloperEvaluation.Application.Events;
using Ambev.DeveloperEvaluation.Application.Abstractions.Persistence;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests.
/// </summary>
public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;


    /// <summary>
    /// Initializes a new instance of CreateSaleHandler.
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    /// <param name="mapper">The mapper instance</param>
    /// <param name="eventPublisher">The event publisher instance</param>
    public CreateSaleHandler(ISaleRepository saleRepository, ILogger<CreateSaleHandler> logger, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _logger = logger;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    /// <summary>
    /// Handles the CreateSaleCommand request.
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    /// <exception cref="DomainException">
    /// Thrown when the command is invalid or the sale cannot be created.
    /// </exception>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        // Validate that at least one item is provided
        if (command.Items == null || command.Items.Count == 0)
        {
            throw new DomainException("Sale must contain at least one item");
        }

        // Create the sale entity using the domain constructor
        var sale = new Sale(
            command.SaleNumber,
            command.CustomerId,
            command.CustomerName,
            command.BranchId,
            command.BranchName);

        // Add items to the sale using the AddItem method
        foreach (var item in command.Items)
        {
            sale.AddItem(
                item.ProductId,
                item.ProductName,
                item.Quantity,
                item.UnitPrice);
        }

        // Add the sale to the repository
        await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publish event
        var saleCreatedEvent = new SaleCreatedEvent
        {
            SaleId = sale.Id,
            SaleNumber = sale.SaleNumber,
            OccurredAt = DateTime.UtcNow
        };
        await _eventPublisher.PublishAsync(saleCreatedEvent, cancellationToken);

        return _mapper.Map<CreateSaleResult>(sale);
    }
}
