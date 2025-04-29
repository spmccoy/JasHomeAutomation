using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using MqttEntities.Common;

namespace MqttEntities.BedRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class BedroomRoomStateSelect : MqttSelect
{
    public BedroomRoomStateSelect() 
        : base("BedRoom", "state", "Bedroom Room State Select")
    {
        AddOption(RoomStates.Off);
        AddOption(RoomStates.Day);
        AddOption(RoomStates.Night);
        AddOption(RoomStates.Sleep);
    }
}