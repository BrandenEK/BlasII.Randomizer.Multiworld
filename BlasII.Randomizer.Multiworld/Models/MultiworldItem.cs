using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Shops;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// An item that can be found in this world
/// </summary>
public abstract class MultiworldItem : Item
{
    /// <summary>
    /// Gets the item sprite
    /// </summary>
    public abstract Sprite GetSprite();

    /// <summary>
    /// Gets the item name
    /// </summary>
    public abstract string GetName();

    /// <summary>
    /// Gets the item description
    /// </summary>
    public abstract string GetDescription();

    /// <summary>
    /// Gets the item value
    /// </summary>
    public abstract ShopValue GetValue();
}

/// <summary>
/// An item in this world that belongs to you
/// </summary>
public class MultiworldSelfItem : MultiworldItem
{
    private readonly Item _item;

    private MultiworldSelfItem(Item item)
    {
        _item = item;
    }

    /// <inheritdoc/>
    public override Sprite GetSprite()
    {
        return _item.GetSprite();
    }

    /// <inheritdoc/>
    public override string GetName()
    {
        return Name;
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return _item.GetDescription();
    }

    /// <inheritdoc/>
    public override ShopValue GetValue()
    {
        return _item.GetValue();
    }

    /// <summary>
    /// Creates a new <see cref="MultiworldSelfItem"/>
    /// </summary>
    public static MultiworldSelfItem Create(ScoutedItemInfo info)
    {
        string itemId = Main.Multiworld.ItemStorage.ServerToInternalId(info.ItemId);
        Item item = Main.Randomizer.ItemStorage[itemId.Contains(',')
            ? itemId[..itemId.IndexOf(',')]
            : itemId];

        return new MultiworldSelfItem(item)
        {
            Id = "MW",
            Name = info.ItemDisplayName,
            Type = ItemType.Invalid,
            Class = item.Class,
            Count = 0
        };
    }
}

/// <summary>
/// An item in this world that belongs to a different player
/// </summary>
public class MultiworldOtherItem : MultiworldItem
{
    private readonly string _player;
    private readonly bool _isTrap;

    private MultiworldOtherItem(string player, bool isTrap)
    {
        _player = player;
        _isTrap = isTrap;
    }

    /// <inheritdoc/>
    public override Sprite GetSprite()
    {
        return Main.Multiworld.IconStorage.GetItemIcon(Class, _isTrap);
    }

    /// <inheritdoc/>
    public override string GetName()
    {
        string separator = Main.Multiworld.LocalizationHandler.Localize("item/for");
        return $"{Name} <color=#F8E4C6>{separator}</color> {_player}";
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        string type = _isTrap ? "trap" : Class.ToString().ToLower();
        string key = $"item/desc/{type}";
        string text = Main.Multiworld.LocalizationHandler.Localize(key);
        return text.Replace("*", _player);
    }

    /// <inheritdoc/>
    public override ShopValue GetValue()
    {
        if (_isTrap)
            return ShopValue.Cherubs;

        return Class switch
        {
            ItemClass.Filler => ShopValue.FillerInventory,
            ItemClass.Useful => ShopValue.UsefulInventory,
            ItemClass.Progression => ShopValue.ProgressionInventory,
            _ => throw new System.Exception($"Invalid item class: {Class}")
        };
    }

    /// <summary>
    /// Creates a new <see cref="MultiworldOtherItem"/>
    /// </summary>
    public static MultiworldOtherItem Create(ScoutedItemInfo info)
    {
        return new MultiworldOtherItem(info.Player.Name, info.Flags.HasFlag(ItemFlags.Trap))
        {
            Id = "MW",
            Name = info.ItemDisplayName,
            Type = ItemType.Invalid,
            Class = info.Flags.HasFlag(ItemFlags.Advancement)
                ? ItemClass.Progression
                : info.Flags.HasFlag(ItemFlags.NeverExclude)
                    ? ItemClass.Useful
                    : ItemClass.Filler,
            Count = 0
        };
    }
}

/// <summary>
/// An item in this world that doesn't exist
/// </summary>
public class MultiworldErrorItem : MultiworldItem
{
    /// <inheritdoc/>
    public override Sprite GetSprite()
    {
        return Main.Multiworld.IconStorage.GetItemIcon(ItemClass.Filler, false);
    }

    /// <inheritdoc/>
    public override string GetName()
    {
        return Name;
    }

    /// <inheritdoc/>
    public override string GetDescription()
    {
        return Name;
    }

    /// <inheritdoc/>
    public override ShopValue GetValue()
    {
        return ShopValue.FillerInventory;
    }

    /// <summary>
    /// Creates a new <see cref="MultiworldErrorItem"/>
    /// </summary>
    public static MultiworldErrorItem Create()
    {
        return new MultiworldErrorItem()
        {
            Id = "MW",
            Name = "Unknown item",
            Type = ItemType.Invalid,
            Class = ItemClass.Filler,
            Count = 0
        };
    }
}
