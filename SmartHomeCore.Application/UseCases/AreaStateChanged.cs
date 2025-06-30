using Microsoft.Extensions.Logging;
using SmartHomeCore.Application.Common;
using SmartHomeCore.Domain.AreaEntity;
using SmartHomeCore.Domain.HouseEntity;

namespace SmartHomeCore.Application.UseCases;

public class AreaStateChanged(House house, IDomainEventDispatcher domainEventDispatcher, ILogger<AreaStateChanged> logger)
    : UseCase(house, domainEventDispatcher, logger)
{
    private string _selectId = null!;
    private string _newState = null!;
    
    public Task HandleAsync(string selectId, string newState)
    {
        _selectId = selectId;
        _newState = newState;
        return base.HandleAsync();
    }

    protected override Task ExecuteCoreLogicAsync()
    {
        if (house.SelectId == _selectId) // check if the area changed is the house
        {
            SetHouseState();
            return Task.CompletedTask;
        }

        var area = house.GetAreaBySelectId(_selectId);
        SetAreaState(area);
        
        return Task.CompletedTask;
    }

    private void SetAreaState(Area? area)
    {
        if (area is null)
        {
            logger.LogDebug("Area {selectId} not found", _selectId);
            return;
        }
        
        var areaState = AreaState.From(_newState);
        if (areaState is null)
        {
            logger.LogDebug("{newState} is not a valid state for area {selectId}", _newState, _selectId);
            return;
        }
        
        area.ChangeState(areaState);
    }
    
    private void SetHouseState()
    {
        var state = HouseState.From(_newState);
        if (state is null)
        {
            logger.LogDebug("{newState} is not a valid state for house.", _newState);
            return;
        }
            
        house.ChangeState(state);
    }
}