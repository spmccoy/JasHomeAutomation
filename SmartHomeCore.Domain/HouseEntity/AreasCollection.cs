using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.AreaEntity;

namespace SmartHomeCore.Domain.HouseEntity;

public class AreasCollection
{
    private readonly List<Area> _areas = [];
    
    public IReadOnlyCollection<Area> Areas => _areas.AsReadOnly();
    
    public Area AddArea(AreaId id, string name)
    {
        if (_areas.Any(r => r.Id == id))
        {
            throw new InvalidOperationException($"Area with ID '{id}' already exists");
        }
        
        var area = new Area(id, name);
        _areas.Add(area);
        return area;
    }
    
    public Area GetArea(AreaId id)
    {
        return _areas.FirstOrDefault(r => r.Id == id)
               ?? throw new EntityNotFoundException(nameof(Area), id);
    }

    public Area GetAreaBySelectId(string selectId)
    {
        return _areas.FirstOrDefault(r => r.SelectId == selectId)
               ?? throw new EntityNotFoundException(nameof(Area), selectId);
    }
}