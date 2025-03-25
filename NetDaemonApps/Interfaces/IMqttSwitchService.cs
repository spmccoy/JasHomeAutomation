using System.Threading.Tasks;
using NetDaemonApps.DomainEntities;

namespace NetDaemonApps.Interfaces;

/// <summary>
/// Defines methods for managing and interacting with MQTT-based switches.
/// </summary>
public interface IMqttSwitchService
{
    /// <summary>
    /// Creates all MQTT-based switches by iterating through a collection of switches
    /// and invoking the creation logic for each one.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task CreateAllAsync();

    /// <summary>
    /// Registers all MQTT-based switches by iterating through a collection of switches
    /// and invoking the registration logic for each one.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RegisterAllAsync();
}