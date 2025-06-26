using Archipelago.MultiClient.Net;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Multiworld;

/// <summary>
/// Scouts all multiworld items on connection
/// </summary>
public class Scouter
{
    // TODO: Better name for this class

    private readonly ServerConnection _connection;
    private readonly Dictionary<string, MultiworldItem> _mappedItems = [];

    /// <summary>
    /// Initializes the scouter connection
    /// </summary>
    public Scouter(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnConnect += OnConnect;
    }

    /// <summary>
    /// Gets the mwitem at the sepecified location
    /// </summary>
    public MultiworldItem GetItemAtLocation(string locationId)
    {
        if (_mappedItems.TryGetValue(locationId, out var item))
            return item;

        ModLog.Error($"Location {locationId} was not scouted");
        return MultiworldErrorItem.Create(locationId);
    }

    private void OnConnect(LoginResult result)
    {
        throw new System.NotImplementedException();
    }
}
