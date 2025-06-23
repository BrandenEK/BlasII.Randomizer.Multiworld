
namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Data for connecting to the AP server
/// </summary>
public class ConnectionInfo(string server, string name, string password)
{
    /// <summary>
    /// The server IP address
    /// </summary>
    public string Server { get; } = server;

    /// <summary>
    /// The player name for the slot
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// The optional password for the room
    /// </summary>
    public string Password { get; } = password;

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
