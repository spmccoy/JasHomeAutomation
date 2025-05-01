using System.Diagnostics.CodeAnalysis;
using MqttEntities.Common;
using MqttEntities.Models;

namespace MqttEntities.MainRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class MainRoomStateSelect : MqttSelect
{
    public MainRoomStateSelect() 
        : base("MainRoom", "state-select", "Main Room State Select")
    {
        AddOption(RoomStates.Off);
        AddOption(RoomStates.Day);
        AddOption(RoomStates.Night);
        AddOption(RoomStates.Tv);
        AddOption(RoomStates.Dawn);
        AddOption(RoomStates.Dusk);
    }
}