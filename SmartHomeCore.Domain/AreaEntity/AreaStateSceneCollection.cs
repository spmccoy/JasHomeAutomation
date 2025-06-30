using SmartHomeCore.Domain.SceneEntity;

namespace SmartHomeCore.Domain.AreaEntity;

public class AreaStateSceneCollection
{
    private readonly Dictionary<AreaState, Scene> _areaStateScenes = new();
    
    public IReadOnlyDictionary<AreaState, Scene> AreaStateScenes => _areaStateScenes;
    
    public void SetSceneForAreaState(AreaState areaState, Scene scene)
    {
        _areaStateScenes[areaState] = scene;
    }
    
    public Scene? GetSceneForAreaState(AreaState areaState)
    {
        return _areaStateScenes.GetValueOrDefault(areaState);
    }
    
    public AreaState[] GetAreaStatesWithScenes()
    {
        return _areaStateScenes.Keys.ToArray();
    }
}