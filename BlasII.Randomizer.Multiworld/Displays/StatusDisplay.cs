using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Randomizer.Multiworld.Models;
using Il2Cpp;
using UnityEngine;
using UnityEngine.UI;

namespace BlasII.Randomizer.Multiworld.Displays;

/// <summary>
/// Displays the connection status on the UI
/// </summary>
public class StatusDisplay
{
    private readonly ServerConnection _connection;

    private Image _image;

    /// <summary>
    /// Initializes the status connection
    /// </summary>
    public StatusDisplay(ServerConnection connection)
    {
        _connection = connection;
    }

    /// <summary>
    /// Updates the connection icon
    /// </summary>
    public void OnUpdate()
    {
        if (!SceneHelper.GameSceneLoaded)
            return;

        if (_image == null)
            _image = CreateImage(Object.FindObjectOfType<UITearsControl>().transform);

        if (_image == null)
        {
            ModLog.Error("Failed to create the connection status image");
            return;
        }

        _image.sprite = Main.Multiworld.IconStorage.GetConnectionIcon(_connection.Connected);
    }

    private Image CreateImage(Transform parent)
    {
        RectTransform connect = UIModder.Create(new RectCreationOptions()
        {
            Name = "Connection",
            Parent = parent,
            Pivot = Vector2.one,
            XRange = Vector2.one,
            YRange = Vector2.zero,
            Position = new Vector2(0, 0),
            Size = new Vector2(64, 64),
        });

        return connect.AddImage();
    }
}
