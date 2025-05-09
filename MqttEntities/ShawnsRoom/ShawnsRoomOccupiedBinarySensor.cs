using MqttEntities.Common;
using MqttEntities.Models;
using NetDaemon.Extensions.MqttEntityManager;

namespace MqttEntities.ShawnsRoom
{
    public class ShawnsRoomOccupiedBinarySensor : MqttBinarySensor
    {
        private readonly IMqttEntityManager _mqttEntityManager;
        private const string Room = "ShawnsRoom";
        private const string State = "occupied";
        private const string Description = "Shawn's Room Occupied";

        public ShawnsRoomOccupiedBinarySensor() : base(Room, State, Description)
        {
        }

        public ShawnsRoomOccupiedBinarySensor(IMqttEntityManager mqttEntityManager) : base(Room, State, Description)
        {
            _mqttEntityManager = mqttEntityManager ?? throw new ArgumentNullException(nameof(mqttEntityManager));
        }

        public async Task UpdateOccupancyStateAsync(bool state)
        {
            if (_mqttEntityManager == null)
            {
                throw new InvalidOperationException("MQTT Entity Manager is not initialized.");
            }
            
            await _mqttEntityManager.SetStateAsync(Id, state ? MqttState.On : MqttState.Off);
        }
    }
}