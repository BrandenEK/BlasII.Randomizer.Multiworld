using BlasII.Framework.UI;
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
            _image = CreateImage();

        if (_image == null)
            return;

        _image.sprite = Main.Multiworld.IconStorage.GetStatusIcon(_connection.Connected);
    }

    private Image CreateImage()
    {
        Transform parent = Object.FindObjectOfType<UITearsControl>()?.transform;

        if (parent == null)
            return null;

        RectTransform connect = UIModder.Create(new RectCreationOptions()
        {
            Name = "mwstatus",
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
