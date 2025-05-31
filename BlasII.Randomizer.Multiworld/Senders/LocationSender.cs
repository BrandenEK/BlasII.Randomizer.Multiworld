using Archipelago.MultiClient.Net;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Multiworld.Senders;

/// <summary>
/// Handles sending checked locations to the AP server
/// </summary>
public class LocationSender
{
    private readonly List<MultiworldLocation> _locationIds = [];

    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes a new LocationSender
    /// </summary>
    public LocationSender(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnConnect += OnConnect;
    }

    /// <summary>
    /// Sends a single checked location to the server
    /// </summary>
    public void Send(ItemLocation location)
    {
        long id = InternalToServerId(location.Id);
        _connection.Session.Locations.CompleteLocationChecksAsync(id);
    }

    private void OnConnect(LoginResult result)
    {
        _locationIds.Clear();
        
        if (result is not LoginSuccessful success)
            return;

        MultiworldLocation[] locations = ((JArray)success.SlotData["locations"]).ToObject<MultiworldLocation[]>();
        _locationIds.AddRange(locations);

        ModLog.Info("Recevied location info from slot data");
        foreach (var location in locations)
        {
            ModLog.Warn($"Internal: {location.InternalId}, Server: {location.ServerId}");
        }
    }

    private string ServerToInternalId(long id)
    {
        return _locationIds.First(x => x.ServerId == id).InternalId;
    }

    private long InternalToServerId(string id)
    {
        return _locationIds.First(x => x.InternalId == id).ServerId;
    }
}
