namespace NetDaemonApps.Extensions;

public static class ClimateEntityExtensions
{
    public static void SetNormal(this ClimateEntity climateEntity)
    {
        climateEntity.SetTemperature(new ClimateSetTemperatureParameters
        {
            TargetTempHigh = 74,
            TargetTempLow = 72,
            HvacMode = "heat_cool"
        });
    }
    
    public static void SetColder(this ClimateEntity climateEntity)
    {
        climateEntity.SetTemperature(new ClimateSetTemperatureParameters
        {
            TargetTempHigh = 72,
            TargetTempLow = 70,
            HvacMode = "heat_cool"
        });
    }
}