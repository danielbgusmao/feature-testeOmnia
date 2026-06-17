using Ambev.DeveloperEvaluation.Application.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events.Handlers;

public class SaleCreatedEventHandler : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger<SaleCreatedEventHandler> _logger;

    public SaleCreatedEventHandler(
        ILogger<SaleCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(SaleCreatedEvent message)
    {
        _logger.LogInformation(
            "SaleCreatedEvent consumed for SaleId {SaleId}",
            message.SaleId);

        await Task.CompletedTask;
    }
}