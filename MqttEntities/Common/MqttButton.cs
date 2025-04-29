using Domain.Entities;

namespace MqttEntities.Common;

public abstract class MqttButton(string groupName, string entityName, string displayName)
    : MqttEntity(HaEntityType.Button, groupName, entityName, displayName);