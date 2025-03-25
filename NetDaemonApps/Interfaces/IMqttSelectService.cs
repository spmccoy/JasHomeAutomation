using System.Threading.Tasks;

namespace NetDaemonApps.Interfaces;

/// <summary>
/// Defines the service for managing MQTT select entities.
/// </summary>
public interface IMqttSelectService
{
    /// <summary>
    /// Asynchronously creates all required MQTT select entities by iterating through the defined selects
    /// and invoking the entity creation process for each.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAllAsync();

    /// <summary>
    /// Asynchronously registers all configured MQTT select entities by iterating through the available selects
    /// and invoking the registration process for each.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterAllAsync();
}