using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using MqttEntities.Common;

namespace MqttEntities.ShawnsRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class ShawnsRoomStateSelect : MqttSelect
{
    public ShawnsRoomStateSelect() 
        : base("ShawnRoom", "state", "Shawn's Room State Select")
    {
        AddOption(RoomStates.Off);
        AddOption(RoomStates.Day);
        AddOption(RoomStates.Night);
        AddOption(RoomStates.Gaming);
        AddOption(RoomStates.SimRacing);
    }
}