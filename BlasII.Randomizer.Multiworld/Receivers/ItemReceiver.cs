using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using System.Collections.Generic;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving items from the AP server
/// </summary>
public class ItemReceiver
{
    private readonly List<ItemInfo> _itemQueue = [];

    /// <summary>
    /// Adds the received item to a queue
    /// </summary>
    public void OnReceiveItem(ReceivedItemsHelper helper)
    {
        lock (ITEM_LOCK)
        {
            ItemInfo item = helper.DequeueItem();

            ModLog.Info($"Receiving item {item.ItemId}");
            _itemQueue.Add(item);
        }
    }

    /// <summary>
    /// Processes items in the queue
    /// </summary>
    public void OnUpdate()
    {
        if (_itemQueue.Count == 0)
            return;

        foreach (ItemInfo item in _itemQueue)
        {
            ModLog.Info($"Processing item {item.ItemId}");

            // Add item to inventory
            // ...

            // Display recevied item
            string itemName = item.ItemDisplayName;
            string playerName = item.Player.Name;
            ModLog.Warn($"Got {itemName} from {playerName}");
        }

        _itemQueue.Clear();
    }

    private static readonly object ITEM_LOCK = new();
}
