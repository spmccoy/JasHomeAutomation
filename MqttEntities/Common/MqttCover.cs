using MqttEntities.Models;

namespace MqttEntities.Common;

public abstract class MqttCover(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Cover, groupName, entityName, displayName);