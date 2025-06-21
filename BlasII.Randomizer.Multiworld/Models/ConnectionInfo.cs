
namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Data for connecting to the AP server
/// </summary>
public class ConnectionInfo
{
    /// <summary>
    /// The server IP address
    /// </summary>
    public string Server { get; set; }

    /// <summary>
    /// The player name for the slot
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The optional password for the room
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Formats the connection info
    /// </summary>
    public override string ToString()
    {
        return $"Connection details to {Server} as {Name}" + (!string.IsNullOrEmpty(Password)
            ? $" [{Password}]"
            : string.Empty);
    }
}
