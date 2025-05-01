using MqttEntities.Common;

namespace MqttEntities.House;

public class OpenGarageDoor() : MqttButton("House", "Garage-Door-Open", "Garage Door Open");
public class CloseGarageDoor() : MqttButton("House", "Garage-Door-Close", "Garage Door Close");