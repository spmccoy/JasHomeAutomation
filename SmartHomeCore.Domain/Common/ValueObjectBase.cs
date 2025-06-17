namespace SmartHomeCore.Domain.Common;

/// <summary>
/// Base class for Value Objects in Domain-Driven Design.
/// Value Objects are immutable objects that contain attributes but have no identity.
/// They are compared by the values of all their fields.
/// </summary>
public abstract class ValueObject
{
    /// <summary>
    /// When overridden in a derived class, returns all components of the value object
    /// that should be used for equality comparison.
    /// </summary>
    /// <returns>An enumerable of objects representing the value components.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    /// <summary>
    /// Determines whether two specified instances of ValueObject are equal.
    /// </summary>
    public static bool operator ==(ValueObject? left, ValueObject right)
    {
        if (left is null && right is null)
            return true;
        
        if (left is null || right is null)
            return false;
            
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of ValueObject are not equal.
    /// </summary>
    public static bool operator !=(ValueObject left, ValueObject right)
    {
        return !(left == right);
    }
}