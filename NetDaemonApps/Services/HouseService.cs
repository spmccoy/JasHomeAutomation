using NetDaemonApps.Interfaces;

namespace NetDaemonApps.Services;

public class HouseService(Entities entities, IPersonService personService, ISunService sunService) : IHouseService
{
    private readonly SelectEntity _select = entities.Select.HouseStateNetdaemon;

    public bool HouseSecure => entities.Cover.Ratgdov25i0a070cDoor.State == HaState.Closed &&
                               entities.Lock.HomeConnect620ConnectedSmartLock.State == HaState.Locked;
    
    public void DetermineAndSetHouseState()
    {
        if (personService.IsNoOneHome())
        {
            _select.SelectOption(RoomState.Away);
            return;
        }

        switch (HouseSecure)
        {
            case true:
                _select.SelectOption(RoomState.HomeSecure);
                return;
            case false:
                _select.SelectOption(RoomState.HomeUnsecured);
                break;
        }
    }

    public void DetermineAndSetOutsideLights()
    {
        var now = DateTime.Now;
        // if it is not dark or past midnight turn off the lights, if on
        if (!sunService.IsDark || now > now.Date.AddHours(0))
        {
            if (entities.Light.PermanentLights.State == HaState.On)
            {
                entities.Light.PermanentLights.TurnOff();
            }

            return;
        }

        var currentState = _select.State;

        if (currentState == RoomState.HomeUnsecured)
        {
            entities.Scene.GoveeToMqttOneClickDefaultPermanentDefault.TurnOn();
        }
        else
        {
            entities.Scene.GoveeToMqttOneClickDefaultPermanentIntruder.TurnOn();
        }
    }
}