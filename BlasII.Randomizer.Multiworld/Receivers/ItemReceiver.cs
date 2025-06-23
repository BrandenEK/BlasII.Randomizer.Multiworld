using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving items from the AP server
/// </summary>
public class ItemReceiver
{
    private readonly List<QueuedItemInfo> _itemQueue = [];

    /// <summary>
    /// The number of items currently received
    /// </summary>
    public int ItemsReceived { get; set; }

    /// <summary>
    /// Adds the received item to a queue
    /// </summary>
    public void OnReceiveItem(ReceivedItemsHelper helper)
    {
        lock (ITEM_LOCK)
        {
            ItemInfo item = helper.DequeueItem();

            _itemQueue.Add(new QueuedItemInfo(item, helper.Index));
        }
    }

    /// <summary>
    /// Processes items in the queue
    /// </summary>
    public void OnUpdate()
    {
        lock (ITEM_LOCK)
        {
            ProcessQueue();
        }
    }

    private void ProcessQueue()
    {
        if (_itemQueue.Count == 0)
            return;

        foreach (QueuedItemInfo info in _itemQueue)
        {
            ModLog.Info($"Processing item {info.ItemName} at index {info.Index} ({ItemsReceived} items received)");

            if (info.Index <= ItemsReceived)
                continue;

            ItemsReceived++;

            // Add item to inventory
            // ...

            // Display recevied item
            ModLog.Warn($"Got {info.ItemName} from {info.PlayerName}"); // temp
        }

        _itemQueue.Clear();
    }

    private static readonly object ITEM_LOCK = new();
}
