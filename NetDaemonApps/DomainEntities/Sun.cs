using System.Collections.Generic;

namespace NetDaemonApps.DomainEntities;

/// <summary>
/// The <c>Sun</c> class represents the state of the sun in terms of day, night, unknown, or unavailable.
/// It maps states received as input into predefined enums that can be used for further processing.
/// </summary>
public class Sun
{
    private readonly Dictionary<string, State> _stateMappings = new Dictionary<string, State>
    {
        {"above_horizon", State.Day},
        {"unknown", State.Unknown},
        {"below_horizon", State.Night},
        {"unavailable", State.Unavailable}
    };

    public enum State
    {
        Unknown,
        Unavailable,
        Day,
        Night
    }
    
    public Sun(string? sunsState)
    {
        if (!_stateMappings.TryGetValue(sunsState ?? string.Empty, out var currentState))
        {
            throw new ArgumentOutOfRangeException(sunsState, "Invalid sun state");
        }

        CurrentState = currentState;
    }

    public State CurrentState { get; private set; }
}