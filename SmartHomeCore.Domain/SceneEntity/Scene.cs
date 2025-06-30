namespace SmartHomeCore.Domain.SceneEntity;

public class Scene
{
    public Scene(SceneId id, string name)
    {
        ArgumentNullException.ThrowIfNull(id);
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(name);
        }
        
        Id = id;
        Name = name;
    }
    
    public SceneId Id { get; }
    
    public string Name { get; }
}