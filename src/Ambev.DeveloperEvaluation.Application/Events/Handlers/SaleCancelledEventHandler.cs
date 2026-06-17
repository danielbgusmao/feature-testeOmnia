using Ambev.DeveloperEvaluation.Application.Events;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events.Handlers;

/// <summary>
/// Handler for processing SaleCancelledEvent messages from Rebus.
/// </summary>
public class SaleCancelledEventHandler : IHandleMessages<SaleCancelledEvent>
{
    private readonly ILogger<SaleCancelledEventHandler> _logger;

    /// <summary>
    /// Initializes a new instance of SaleCancelledEventHandler.
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public SaleCancelledEventHandler(
        ILogger<SaleCancelledEventHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the SaleCancelledEvent message.
    /// </summary>
    /// <param name="message">The sale cancelled event</param>
    /// <returns>A completed task</returns>
    public async Task Handle(SaleCancelledEvent message)
    {
        _logger.LogInformation(
            "SaleCancelledEvent consumed for SaleId {SaleId}",
            message.SaleId);

        await Task.CompletedTask;
    }
}
