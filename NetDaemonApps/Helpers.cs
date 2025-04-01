namespace NetDaemonApps;

public static class Helpers
{
    /// <summary>
    /// Cancels and disposes of the provided schedule if it is not null.
    /// </summary>
    /// <param name="schedulerSubscription">The disposable schedule to be canceled and disposed.</param>
    public static void CancelSchedule(IDisposable? schedulerSubscription)
    {
        if (schedulerSubscription != null)
        {
            schedulerSubscription.Dispose();
            schedulerSubscription = null;
        }
    }
}