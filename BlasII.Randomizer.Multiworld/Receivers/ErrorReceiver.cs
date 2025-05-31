using BlasII.ModdingAPI;
using System;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving errors from the AP socket
/// </summary>
public class ErrorReceiver
{
    /// <summary>
    /// Displays the formatted error
    /// </summary>
    public void Handle(Exception ex, string message)
    {
        ModLog.Error($"Received socket error: {message}");
        ModLog.Error(ex);
    }
}
