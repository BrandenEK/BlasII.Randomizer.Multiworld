using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Storages;

/// <summary>
/// Stores sprites
/// </summary>
public class IconStorage
{
    private readonly Sprite _itemSprite;
    private readonly Sprite[] _statusSprites;

    /// <summary>
    /// Loads all required icons
    /// </summary>
    public IconStorage(FileHandler file)
    {
        ModLog.Info("Loading multiworld item icon");
        file.LoadDataAsSprite("mwitem.png", out _itemSprite);

        ModLog.Info("Loading multiworld status icons");
        file.LoadDataAsFixedSpritesheet("mwstatus.png", new Vector2(22, 22), out _statusSprites);
    }

    /// <summary>
    /// Gets the mwitem icon
    /// </summary>
    public Sprite GetItemSprite()
    {
        return _itemSprite;
    }

    /// <summary>
    /// Gets the connected or disconnected icon
    /// </summary>
    public Sprite GetStatusIcon(bool status)
    {
        return _statusSprites[status ? 0 : 1];
    }
}
