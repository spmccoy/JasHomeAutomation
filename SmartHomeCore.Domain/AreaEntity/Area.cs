using System.Text.RegularExpressions;
using SmartHomeCore.Domain.Common;
using SmartHomeCore.Domain.SceneEntity;

namespace SmartHomeCore.Domain.AreaEntity;

public class Area : Entity, IHasState<AreaState>
{
    private readonly AreaStateSceneCollection _areaStateScenes;

    public Area(AreaId id, string name)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(name);
        }
        
        Id = id;
        Name = name;
        _areaStateScenes = new AreaStateSceneCollection();
    }

    public AreaId Id { get; }

    public string Name { get; }
    
    public AreaState CurrentState { get; private set; } = AreaState.Unknown;

    public string SelectId => GenerateSelectId(Name);

    public void SetSceneForAreaState(AreaState areaState, Scene scene) => _areaStateScenes.SetSceneForAreaState(areaState, scene);

    public Scene? GetSceneForAreaState(AreaState areaState) => _areaStateScenes.GetSceneForAreaState(areaState);

    public AreaState[] GetAreaStatesWithScenes() => _areaStateScenes.GetAreaStatesWithScenes();
    
    public void ChangeState(AreaState newState)
    {
        var oldState = CurrentState;

        if (oldState == newState)
        {
            return;
        }

        CurrentState = newState;
        var sceneForNewState = GetSceneForAreaState(newState);
        AddDomainEvent(new AreaStateChangedEvent(Name, oldState, newState, sceneForNewState));
    }
}