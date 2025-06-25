using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Storages;

/// <summary>
/// Stores sprites
/// </summary>
public class IconStorage
{
    /// <summary>
    /// The sprite for a MultiworldItem
    /// </summary>
    public Sprite ItemSprite { get; }

    /// <summary>
    /// Loads all required icons
    /// </summary>
    public IconStorage(FileHandler file)
    {
        ModLog.Info("Loading multiworld item icon");
        file.LoadDataAsSprite("mwitem.png", out Sprite item);

        ItemSprite = item;
    }
}
