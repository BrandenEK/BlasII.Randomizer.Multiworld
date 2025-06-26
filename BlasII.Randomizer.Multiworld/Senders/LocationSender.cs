using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Senders;

/// <summary>
/// Handles sending checked locations to the AP server
/// </summary>
public class LocationSender
{
    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes a new LocationSender
    /// </summary>
    public LocationSender(ServerConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Sends a single checked location to the server
    /// </summary>
    public void Send(ItemLocation location)
    {
        long id = Main.Multiworld.LocationStorage.InternalToServerId(location.Id);
        _connection.Session.Locations.CompleteLocationChecksAsync(id);
    }
}
