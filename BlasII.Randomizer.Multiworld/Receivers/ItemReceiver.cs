using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using Il2CppTGK.Game;
using System.Collections.Generic;
using System.Linq;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving items from the AP server
/// </summary>
public class ItemReceiver
{
    private readonly List<QueuedItemInfo> _itemQueue = [];
    private readonly ServerConnection _connection;

    /// <summary>
    /// The number of items currently received
    /// </summary>
    public int ItemsReceived { get; set; }

    /// <summary>
    /// Initializes a new ItemReceiver
    /// </summary>
    public ItemReceiver(ServerConnection connection)
    {
        _connection = connection;
    }

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
        if (!SceneHelper.GameSceneLoaded || CoreCache.Input.InputBlocked)
            return;

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
            string itemName = info.Item.ItemDisplayName;
            string playerName = info.Item.Player.Name;

            ModLog.Info($"Processing item {itemName} at index {info.Index} ({ItemsReceived} items received)");

            if (info.Index <= ItemsReceived)
                continue;

            Item item = FindItemByName(itemName);
            ModLog.Info($"Giving item {itemName} from {playerName}");
            ItemsReceived++;

            if (info.Item.Player.Slot != _connection.Session.ConnectionInfo.Slot)
            {
                // Display recevied item if its from a different player
                CoreCache.UINavigationHelper.ShowItemPopup(
                    Main.Multiworld.LocalizationHandler.Localize("item/given"),
                    $"{itemName} <color=#F8E4C6>{Main.Multiworld.LocalizationHandler.Localize("item/from")}</color> {playerName}",
                    item.GetSprite(),
                    false);
            }

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
