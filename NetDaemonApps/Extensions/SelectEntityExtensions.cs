using MqttEntities.Models;

namespace NetDaemonApps.Extensions;

public static class SelectEntityExtensions
{
    public static void SelectOff(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Off.ToString());
    }
    
    public static void SelectAway(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Away.ToString());
    }
    
    public static void SelectHomeSecure(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.HomeSecure.ToString());
    }
    
    public static void SelectHomeUnSecure(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.HomeUnsecured.ToString());
    }
    
    public static void SelectDay(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Day.ToString());
    }
    
    public static void SelectNight(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Night.ToString());
    }
    
    public static void SelectDawn(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Dawn.ToString());
    }
    
    public static void SelectDusk(this SelectEntity selectEntity)
    {
        selectEntity.SelectOption(RoomStates.Dusk.ToString());
    }
}