using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Displays;

/// <summary>
/// Displays a message when disconnected from the AP server
/// </summary>
public class DisconnectDisplay
{
    private readonly ServerConnection _connection;

    private bool _waitNextFrame = false;

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
        ModLog.Error("on disconnect called");
        _waitNextFrame = true;
    }

    /// <summary>
    /// Processes the display in the main thread
    /// </summary>
    public void OnUpdate()
    {
        if (!_waitNextFrame)
            return;

        _waitNextFrame = false;
        ModLog.Warn("Disconnected from the AP server!");
    }
}
