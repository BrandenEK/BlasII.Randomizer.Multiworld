using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;
using System.Linq;

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

            Item item = FindItemByName(info.ItemName);
            ItemsReceived++;

            // Display recevied item
            ModLog.Warn($"Got {info.ItemName} from {info.PlayerName}"); // temp

            // Add item to inventory
            item.GiveReward();
        }

        _itemQueue.Clear();
    }

    private Item FindItemByName(string name)
    {
        Item item = Main.Randomizer.ItemStorage.AsSequence.FirstOrDefault(x => x.Name == name);
        // This does not get quest items with a duplicate name

        if (item == null)
        {
            ModLog.Error($"{name} is not a valid item name");
            return Main.Randomizer.ItemStorage.InvalidItem;
        }

        return item;
    }

    private static readonly object ITEM_LOCK = new();
}
