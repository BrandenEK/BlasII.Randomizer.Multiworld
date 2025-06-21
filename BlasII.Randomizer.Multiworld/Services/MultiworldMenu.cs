using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using BlasII.ModdingAPI;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Services;

/// <summary>
/// Allows entering required AP info before the game starts
/// </summary>
public class MultiworldMenu : ModMenu
{
    /// <summary>
    /// Maximum priority
    /// </summary>
    protected override int Priority { get; } = int.MaxValue;

    /// <summary>
    /// Creates all the UI for the menu
    /// </summary>
    protected override void CreateUI(Transform ui)
    {
        var text = new TextCreator(this)
        {
            LineSize = 400,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
        };

        _setServer = text.CreateOption("server", ui, new Vector2(0, 150), "option/server", false, true, 64);
        _setName = text.CreateOption("name", ui, new Vector2(0, 0), "option/name", false, true, 64);
        _setPassword = text.CreateOption("password", ui, new Vector2(0, -150), "option/password", false, true, 64);
    }

    /// <summary>
    /// Restore client settings to menu
    /// </summary>
    public override void OnStart()
    {
        // Get save file values from somewhere

        _setServer.CurrentValue = string.Empty;
        _setName.CurrentValue = string.Empty;
        _setPassword.CurrentValue = string.Empty;
    }

    /// <summary>
    /// Store client settings from menu
    /// </summary>
    public override void OnFinish()
    {
        base.OnFinish();
    }

    private TextOption _setServer;
    private TextOption _setName;
    private TextOption _setPassword;

    private const int TEXT_SIZE = 56;
    private readonly Color SILVER = new Color32(192, 192, 192, 255);
    private readonly Color YELLOW = new Color32(255, 231, 65, 255);
}
