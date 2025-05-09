namespace NetDaemonApps.Extensions;

public static class SensorEntityExtensions
{
    /// <summary>
    /// Determines if the state of the given sensor entity is recent based on the specified time span threshold.
    /// </summary>
    /// <param name="entity">The sensor entity to evaluate.</param>
    /// <param name="threshold">The time span threshold to determine if the sensor state is recent.</param>
    /// <returns>
    /// A boolean indicating whether the state is considered recent.
    /// Returns null if the state could not be parsed as a date and time.
    /// </returns>
    public static bool? IsRecent(this SensorEntity entity, TimeSpan threshold)
    {
        var isParsingSuccessful = DateTime.TryParse(entity.State, out var parsedDateTime);
        if (!isParsingSuccessful)
        {
            return null;
        }
        return DateTime.Now - parsedDateTime < threshold;
    }
}