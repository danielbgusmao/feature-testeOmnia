using Ambev.DeveloperEvaluation.Application.Events;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Rebus.Bus;

namespace Ambev.DeveloperEvaluation.ORM.Events;

/// <summary>
/// Publisher for domain events using Rebus with retry policy.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly ILogger<EventPublisher> _logger;
    private readonly IBus _bus;
    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// Initializes a new instance of EventPublisher.
    /// </summary>
    /// <param name="logger">The logger instance</param>
    /// <param name="bus">The Rebus bus instance</param>
    public EventPublisher(ILogger<EventPublisher> logger, IBus bus)
    {
        _logger = logger;
        _bus = bus;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    var eventName = context.ContainsKey("EventName")
                        ? context["EventName"]?.ToString()
                        : "Unknown";

                    _logger.LogWarning(
                        exception,
                        "Retry attempt {RetryCount} for event {EventName} after {Delay}ms",
                        retryCount,
                        eventName,
                        timespan.TotalMilliseconds);
                });
    }

    /// <summary>
    /// Publishes an event asynchronously with retry policy.
    /// </summary>
    /// <typeparam name="TEvent">The type of event to publish</typeparam>
    /// <param name="event">The event to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class
    {
        if (@event is null)
        {
            _logger.LogError("Attempted to publish a null event");
            throw new ArgumentNullException(nameof(@event), "Event cannot be null");
        }

        var eventName = typeof(TEvent).Name;
        var context = new Context { { "EventName", eventName } };

        _logger.LogInformation("Publishing event {EventName}", eventName);

        try
        {
            var attempt = 0;

            await _retryPolicy.ExecuteAsync(
                async (_, ct) =>
                {

                    attempt++;

                    _logger.LogInformation(
                        "TEST RETRY - Event {EventName} - Attempt {Attempt}",
                        eventName,
                        attempt);

                    if (attempt < 3)
                    {
                        throw new Exception("Simulated transient failure");
                    }

                    ct.ThrowIfCancellationRequested();

                    await _bus.Publish(@event);

                    _logger.LogInformation(
                        "Event {EventName} published successfully",
                        eventName);
                },
                context,
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event {EventName}", eventName);
            throw;
        }
    }
}