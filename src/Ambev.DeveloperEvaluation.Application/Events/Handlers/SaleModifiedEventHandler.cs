using Ambev.DeveloperEvaluation.Application.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events.Handlers;

/// <summary>
/// Handler for processing SaleModifiedEvent messages from Rebus.
/// </summary>
public class SaleModifiedEventHandler : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger<SaleModifiedEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleModifiedEventHandler.
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleModifiedEventHandler(
        ILogger<SaleModifiedEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleModifiedEvent message.
    /// </summary>
    /// <param name="message">The sale modified event</param>
    /// <returns>A completed task</returns>
    public async Task Handle(SaleModifiedEvent message)
    {
        _logger.LogInformation(
            "SaleModifiedEvent consumed for SaleId {SaleId}",
            message.SaleId);

        await Task.CompletedTask;
    }
}
