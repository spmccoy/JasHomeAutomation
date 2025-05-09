using MqttEntities.Common;
using NetDaemon.Extensions.MqttEntityManager;

namespace MqttEntities.ShawnsRoom
{
    public class LastMotionInShawnsRoomSensor : MqttSensor
    {
        private readonly IMqttEntityManager? _mqttEntityManager;

        private const string Area = "ShawnsRoom";
        private const string ObjectId = "last_motion";
        private const string Name = "Shawn's Room Last Motion";

        public LastMotionInShawnsRoomSensor() 
            : base(Area, ObjectId, Name, Models.DeviceClass.TimeStamp)
        {
        }

        public LastMotionInShawnsRoomSensor(IMqttEntityManager mqttEntityManager)
            : base(Area, ObjectId, Name, Models.DeviceClass.TimeStamp)
        {
            _mqttEntityManager = mqttEntityManager ?? throw new ArgumentNullException(nameof(mqttEntityManager));
        }

        public async Task UpdateMotionStateAsync()
        {
            if (_mqttEntityManager == null)
            {
                throw new InvalidOperationException("MQTT Entity Manager is not initialized.");
            }

            var now = DateTime.Now.ToString("o");
            await _mqttEntityManager.SetStateAsync(Id, now);
        }
    }
}