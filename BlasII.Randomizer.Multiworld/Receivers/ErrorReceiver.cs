using BlasII.ModdingAPI;
using System;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving errors from the AP server
/// </summary>
public class ErrorReceiver
{
    /// <summary>
    /// Displays the formatted error
    /// </summary>
    public void OnReceiveError(Exception ex, string message)
    {
        ModLog.Error($"Received socket error ({message}): {ex}");
    }
}
