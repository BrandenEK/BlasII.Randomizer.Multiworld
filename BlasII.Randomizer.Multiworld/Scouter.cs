using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;
using System.Linq;

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
        return MultiworldErrorItem.Create();
    }

    private async void OnConnect(LoginResult result)
    {
        _mappedItems.Clear();

        if (result is not LoginSuccessful)
            return;

        Dictionary<long, ScoutedItemInfo> scouts = await _connection.Session.Locations.ScoutLocationsAsync(
                HintCreationPolicy.None,
                Main.Multiworld.LocationStorage.ServerIds.ToArray());

        foreach (var info in scouts.Values)
        {
            string internalId = Main.Multiworld.LocationStorage.ServerToInternalId(info.LocationId);
            string itemName = info.ItemDisplayName;
            bool progression = info.Flags.HasFlag(ItemFlags.Advancement) || info.Flags.HasFlag(ItemFlags.Trap);

            MultiworldItem item = info.Player.Slot == _connection.Session.ConnectionInfo.Slot
                ? MultiworldSelfItem.Create(itemName, Main.Randomizer.ItemStorage["RB01"])
                : MultiworldOtherItem.Create(itemName, progression, info.Player.Name);

            _mappedItems.Add(internalId, item);
        }

        ModLog.Info($"Scouted {_mappedItems.Count} locations");
    }
}
