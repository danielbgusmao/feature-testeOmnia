using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Application.Abstractions.Persistence;

/// <summary>
/// Abstraction for the database context.
/// </summary>
public interface IDefaultContext
{
    /// <summary>
    /// Gets the Users DbSet.
    /// </summary>
    DbSet<User> Users { get; }

    /// <summary>
    /// Gets the Sales DbSet.
    /// </summary>
    DbSet<Sale> Sales { get; }

    /// <summary>
    /// Gets the SaleItems DbSet.
    /// </summary>
    DbSet<SaleItem> SaleItems { get; }

    /// <summary>
    /// Saves changes to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
