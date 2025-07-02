using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Displays;

/// <summary>
/// Displays a message when disconnected from the AP server
/// </summary>
public class DisconnectDisplay
{
    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes a new DisconnectDisplay
    /// </summary>
    public DisconnectDisplay(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnDisconnect += OnDisconnect;
    }

    private void OnDisconnect()
    {
        ModLog.Warn("Disconnected from the AP server!");
    }
}
