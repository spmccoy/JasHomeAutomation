using System.Reflection;

namespace MqttEntities.Common;

public class MqttEntityManager
{
    public static MqttEntity[] GetSubtypeInstancesOfMqttEntity()
    {
        var mqttEntitySubtypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(MqttEntity)) && !t.IsAbstract) // Find all non-abstract subclasses
            .ToArray();

        var instances = new List<MqttEntity>();
        foreach (var type in mqttEntitySubtypes)
        {
            try
            {
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                {
                    instances.Add((MqttEntity)instance); // Add the instance as the specific type
                }
            }
            catch
            {
                Console.WriteLine($"Could not instantiate type: {type.FullName}");
            }
        }

        return instances.ToArray();
    }
}