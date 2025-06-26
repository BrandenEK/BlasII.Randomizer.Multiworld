using BlasII.Randomizer.Models;
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

    /// <summary>
    /// Creates a new <see cref="MultiworldSelfItem"/>
    /// </summary>
    public static MultiworldSelfItem Create(string id, string name, Item item)
    {
        return new MultiworldSelfItem(item)
        {
            Id = id,
            Name = name,
            Type = ItemType.Invalid,
            Progression = item.Progression,
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

    private MultiworldOtherItem(string player)
    {
        _player = player;
    }

    /// <inheritdoc/>
    public override Sprite GetSprite()
    {
        return Main.Multiworld.IconStorage.ItemSprite;
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
        // TODO: Use new class system
        string key = $"item/desc/{(Progression ? "progression" : "filler")}";
        return Main.Multiworld.LocalizationHandler.Localize(key);
    }

    /// <summary>
    /// Creates a new <see cref="MultiworldOtherItem"/>
    /// </summary>
    public static MultiworldOtherItem Create(string id, string name, bool progression, string player)
    {
        return new MultiworldOtherItem(player)
        {
            Id = id,
            Name = name,
            Type = ItemType.Invalid,
            Progression = progression,
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
        return Main.Multiworld.IconStorage.ItemSprite;
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

    /// <summary>
    /// Creates a new <see cref="MultiworldErrorItem"/>
    /// </summary>
    public static MultiworldErrorItem Create(string id)
    {
        return new MultiworldErrorItem()
        {
            Id = id,
            Name = "Unknown item",
            Type = ItemType.Invalid,
            Progression = false,
            Count = 0
        };
    }
}
