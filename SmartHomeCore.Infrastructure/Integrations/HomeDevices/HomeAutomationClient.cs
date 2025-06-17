using Integrations.HomeDevices.HomeAssistantGenerated;
using SmartHomeCore.Application.Common;

namespace SmartHomeCore.Infrastructure.Integrations.HomeDevices;

public class HomeAutomationClient : IHomeAutomationClient
{
    private readonly ButtonEntities _buttons;

    public HomeAutomationClient(
        ButtonEntities buttons)
    {
        _buttons = buttons;
    }
    
    public Task CloseGarageAsync()
    {
        _buttons.HouseGarageDoorCloseNetdaemon.Press();
        return Task.CompletedTask;
    }

    public Task OpenGarageAsync()
    {
        _buttons.HouseGarageDoorOpenNetdaemon.Press();
        return Task.CompletedTask;
    }
}