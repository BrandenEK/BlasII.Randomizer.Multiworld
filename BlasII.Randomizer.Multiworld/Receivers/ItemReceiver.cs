using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using Il2CppTGK.Game;
using System.Collections.Generic;

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
        _connection.OnDisconnect += ClearQueue;
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

    private void ClearQueue()
    {
        lock (ITEM_LOCK)
        {
            _itemQueue.Clear();
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

            Item item = FindItemById(info.Item.ItemId);
            ModLog.Info($"Giving item {item.Id} from {playerName}");
            ItemsReceived++;

            if (info.Item.Player.Slot != _connection.Session.ConnectionInfo.Slot)
            {
                // Display recevied item if its from a different player
                string message = Main.Multiworld.LocalizationHandler.Localize("item/popup");
                string name = item.IsValid()
                    ? $"{itemName} <color=#F8E4C6>{Main.Multiworld.LocalizationHandler.Localize("item/from")}</color> {playerName}"
                    : item.GetName();
                
                //string message = $"{Main.Multiworld.LocalizationHandler.Localize("item/from")} <color=#FFE38F>{playerName}</color>";
                //string name = item.IsValid() ? itemName : item.GetName();

                Main.Randomizer.ItemDisplayer.Show(message, name, item.GetSprite());
            }

            // Add item to inventory
            item.GiveReward();
        }

        _itemQueue.Clear();
    }

    private Item FindItemById(long id)
    {
        string itemId = Main.Multiworld.ItemStorage.ServerToInternalId(id);

        if (!itemId.Contains(','))
            return Main.Randomizer.ItemStorage[itemId];

        string[] itemIds = itemId.Split(',');

        for (int i = 0; i < itemIds.Length; i++)
        {
            if (!Main.Randomizer.ItemHandler.IsItemCollected(itemIds[i]))
                return Main.Randomizer.ItemStorage[itemIds[i]];
        }

        ModLog.Error($"All of {itemId} has already been collected");
        return Main.Randomizer.ItemStorage.InvalidItem;
    }

    private static readonly object ITEM_LOCK = new();
}
