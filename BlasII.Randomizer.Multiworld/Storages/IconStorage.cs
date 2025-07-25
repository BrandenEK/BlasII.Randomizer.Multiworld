using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using BlasII.Randomizer.Models;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Storages;

/// <summary>
/// Stores sprites
/// </summary>
public class IconStorage
{
    private readonly Sprite[] _itemSprites;
    private readonly Sprite[] _statusSprites;

    /// <summary>
    /// Loads all required icons
    /// </summary>
    public IconStorage(FileHandler file)
    {
        ModLog.Info("Loading multiworld item icon");
        file.LoadDataAsFixedSpritesheet("mwitems.png", new Vector2(30, 30), out _itemSprites);

        ModLog.Info("Loading multiworld status icons");
        file.LoadDataAsFixedSpritesheet("mwstatus.png", new Vector2(22, 22), out _statusSprites);
    }

    /// <summary>
    /// Gets an item icon based on class
    /// </summary>
    public Sprite GetItemIcon(Item.ItemClass @class, bool isTrap)
    {
        return isTrap ? _itemSprites[3] : _itemSprites[(int)@class];
    }

    /// <summary>
    /// Gets the connected or disconnected icon
    /// </summary>
    public Sprite GetStatusIcon(bool status)
    {
        return _statusSprites[status ? 0 : 1];
    }
}
