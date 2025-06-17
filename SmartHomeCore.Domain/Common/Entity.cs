namespace SmartHomeCore.Domain.Common;

public abstract class Entity
{
    private readonly List<DomainEvent> _domainEvents = new();
    
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    /// <summary>
    /// Collects all domain events from the current entity and its child entities,
    /// aggregates them into a single list, and clears the domain events from the originating entities.
    /// </summary>
    /// <returns>
    /// A list of all collected domain events from the current entity and its child entities.
    /// </returns>
    public List<DomainEvent> CollectAndClearAllDomainEvents()
    {
        var allEvents = new List<DomainEvent>();
    
        CollectAndClearEvents(this, allEvents);
    
        CollectEventsFromChildEntities(allEvents);
    
        return allEvents;
    }

    private void CollectEventsFromChildEntities(List<DomainEvent> allEvents)
    {
        foreach (var property in GetEntityProperties())
        {
            var value = property.GetValue(this);
            if (value == null)
                continue;
            
            ProcessPropertyValue(value, allEvents);
        }
    }

    private void ProcessPropertyValue(object value, List<DomainEvent> allEvents)
    {
        switch (value)
        {
            case Entity entity:
                CollectAndClearEvents(entity, allEvents);
                break;
            case IEnumerable<Entity> entityCollection:
                CollectAndClearEvents(entityCollection, allEvents);
                break;
        }
    }

    private IEnumerable<System.Reflection.PropertyInfo> GetEntityProperties()
    {
        return GetType().GetProperties()
            .Where(p => IsEntityProperty(p) || IsEntityCollectionProperty(p));
    }

    private bool IsEntityProperty(System.Reflection.PropertyInfo property)
    {
        return typeof(Entity).IsAssignableFrom(property.PropertyType) && 
               property.PropertyType != GetType();
    }

    private bool IsEntityCollectionProperty(System.Reflection.PropertyInfo property)
    {
        if (!property.PropertyType.IsGenericType)
            return false;
        
        var entityType = typeof(Entity);
        var enumerableType = typeof(IEnumerable<>).MakeGenericType(entityType);
        var collectionType = typeof(ICollection<>).MakeGenericType(entityType);
        var listType = typeof(IList<>).MakeGenericType(entityType);
    
        return enumerableType.IsAssignableFrom(property.PropertyType) ||
               collectionType.IsAssignableFrom(property.PropertyType) ||
               listType.IsAssignableFrom(property.PropertyType);
    }
    
    private void CollectAndClearEvents(Entity entity, List<DomainEvent> eventCollection)
    {
        eventCollection.AddRange(entity.DomainEvents);
        entity.ClearDomainEvents();
    }
    
    private void CollectAndClearEvents(IEnumerable<Entity> entities, List<DomainEvent> eventCollection)
    {
        foreach (var entity in entities)
        {
            eventCollection.AddRange(entity.DomainEvents);
            entity.ClearDomainEvents();
        }
    }
}
