using System.Threading.Tasks;

namespace NetDaemonApps.Interfaces;

/// <summary>
/// Provides functionality to manage the lifecycle of MQTT scenes.
/// </summary>
public interface IMqttSceneService
{
    /// <summary>
    /// Creates all configured MQTT scenes asynchronously by iterating through the available scenes
    /// and invoking scene creation for each.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAllAsync();

    /// <summary>
    /// Registers all configured MQTT scenes asynchronously by iterating through the available scenes
    /// and invoking registration for each.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterAllAsync();
}