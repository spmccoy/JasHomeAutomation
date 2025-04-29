using System.Diagnostics.CodeAnalysis;
using Domain.Entities;

namespace MqttEntities.JustinsRoom;

/// <summary>
/// Applies room settings for a given state.
/// </summary>
[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public class JustinsRoomStateSelect : MqttSelect
{
    public JustinsRoomStateSelect() 
        : base("JustinRoom", "state", "Justin's Room State Select")
    {
        AddOption(RoomStates.Off);
        AddOption(RoomStates.Day);
    }
}