using MqttEntities.Models;

namespace MqttEntities.Common;

public abstract class MqttBinarySensor(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.BinarySensor, groupName, entityName, displayName);