using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Displays;

/// <summary>
/// Displays the connection status on the UI
/// </summary>
public class StatusDisplay
{
    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes the status connection
    /// </summary>
    public StatusDisplay(ServerConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Updates the connection icon
    /// </summary>
    public void OnUpdate()
    {

    }
}
