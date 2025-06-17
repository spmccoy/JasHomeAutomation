namespace SmartHomeCore.Domain.Common;

public class EntityNotFoundException(string entityType, string entityId)
    : Exception($"{entityType} with ID '{entityId}' does not exist in this house")
{
    public string EntityType { get; } = entityType;
    public string EntityId { get; } = entityId;
}