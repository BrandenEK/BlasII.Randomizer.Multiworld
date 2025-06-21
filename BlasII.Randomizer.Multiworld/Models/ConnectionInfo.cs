
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
}
