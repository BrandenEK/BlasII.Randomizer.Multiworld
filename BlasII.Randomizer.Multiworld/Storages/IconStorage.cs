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
    private readonly Sprite[] _connectSprites;

    /// <summary>
    /// Loads all required icons
    /// </summary>
    public IconStorage(FileHandler file)
    {
        ModLog.Info("Loading multiworld item icon");
        file.LoadDataAsSprite("mwitem.png", out _itemSprite);

        ModLog.Info("Loading multiworld connection icons");
        file.LoadDataAsFixedSpritesheet("mwconnection.png", new Vector2(30, 30), out _connectSprites);
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
    public Sprite GetConnectionIcon(bool connected)
    {
        return _connectSprites[connected ? 0 : 1];
    }
}
