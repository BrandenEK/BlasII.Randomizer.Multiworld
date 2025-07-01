using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;
using System.Linq;

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
    public void Send(string location)
    {
        if (!_connection.Connected)
            return;

        long id = Main.Multiworld.LocationStorage.InternalToServerId(location);
        _connection.Session.Locations.CompleteLocationChecksAsync(id);
    }

    /// <summary>
    /// Sends multiple checked locations to the server
    /// </summary>
    public void SendMultiple(IEnumerable<string> locations)
    {
        if (!_connection.Connected)
            return;

        long[] ids = locations.Select(Main.Multiworld.LocationStorage.InternalToServerId).ToArray();
        _connection.Session.Locations.CompleteLocationChecksAsync(ids);
    }
}
