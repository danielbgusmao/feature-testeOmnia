using Ambev.DeveloperEvaluation.Common.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Common;

/// <summary>
/// Base class for all domain entities.
/// Provides common functionality including ID generation and comparison.
/// </summary>
public class BaseEntity : IComparable<BaseEntity>
{
    /// <summary>
    /// Gets the unique identifier for this entity.
    /// Automatically initialized with a new GUID when the entity is created.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Validates this entity asynchronously using the registered validator.
    /// </summary>
    /// <returns>A collection of validation error details if validation fails.</returns>
    public Task<IEnumerable<ValidationErrorDetail>> ValidateAsync()
    {
        return Validator.ValidateAsync(this);
    }

    /// <summary>
    /// Compares this entity with another entity by their ID.
    /// </summary>
    /// <param name="other">The other entity to compare with.</param>
    /// <returns>A negative value if this ID is less than the other ID, 0 if equal, or a positive value if greater.</returns>
    public int CompareTo(BaseEntity? other)
    {
        if (other == null)
        {
            return 1;
        }

        return other!.Id.CompareTo(Id);
    }
}
