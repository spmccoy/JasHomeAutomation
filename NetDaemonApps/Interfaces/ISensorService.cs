using System.Collections.Generic;
using NetDaemon.HassModel.Entities;

namespace NetDaemonApps.Interfaces;

public interface ISensorService
{
    IEnumerable<Entity> GetAllWaterLeakSensors();
}