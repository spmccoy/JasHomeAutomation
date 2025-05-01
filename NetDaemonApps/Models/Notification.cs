using System.Collections.Generic;

namespace NetDaemonApps.Models;

public class Notification
{
    public List<NotifiableDevice> Devices { get; init; } = [];
    
    public string? Title { get; init; }
    
    public bool HasTitle => !string.IsNullOrWhiteSpace(Title);
    
    public string? Text { get; init; }
    
    public bool HasText => !string.IsNullOrWhiteSpace(Text);
    
    public string? Tts { get; init; }

    public bool HasTts => !string.IsNullOrWhiteSpace(Tts);
}