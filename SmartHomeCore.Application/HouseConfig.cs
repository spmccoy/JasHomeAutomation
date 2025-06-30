using Microsoft.Extensions.DependencyInjection;
using SmartHomeCore.Domain.HouseEntity;
using SmartHomeCore.Domain.PersonEntity;
using SmartHomeCore.Domain.AreaEntity;
using SmartHomeCore.Domain.DoorEntity;
using SmartHomeCore.Domain.SceneEntity;

namespace SmartHomeCore.Application;

public static class HouseConfig
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<House>(_ =>
        {
            var house = new House();
            
            house.AddPerson(PersonId.From("shawn"), "Shawn");
            
            house.ConfigShawnsRoomArea();
            
            house.AddDoor(DoorId.From("ratgdov25i_0a070c_door"), "Garage door", true, false);
            house.AddDoor(DoorId.From("home_connect_620_connected_smart_lock"), "Front door", false, true);
            
            return house;
        });
    }
    
    private static void ConfigShawnsRoomArea(this House house)
    {
        const string areaName = "Shawn's office";
        var areaId = AreaId.From("shawns_office");
        
        var shawnsOfficeArea = house.AddArea(areaId, areaName);
        
        shawnsOfficeArea.SetSceneForAreaState(
            AreaState.Day, 
            new Scene(SceneId.From("shawn_s_office_day"),"Shawn's office day scene"));
    }
}

/* TODO: add areas, here are their ids
 public static readonly HaArea ShawnRoom = new("shawns_office");
   public static readonly HaArea MainRoom = new("dining_room");
   public static readonly HaArea JustinsRoom = new("justins_office");
   public static readonly HaArea Bedroom = new("bedroom");
   public static readonly HaArea Outside = new("front_yard");*/