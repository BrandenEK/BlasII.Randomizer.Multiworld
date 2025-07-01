using Archipelago.MultiClient.Net;
using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.ModdingAPI.Input;
using BlasII.Randomizer.Multiworld.Models;
using Il2CppTGK.Game.Components.UI;
using MelonLoader;
using System.Collections;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Services;

/// <summary>
/// Allows entering required AP info before the game starts
/// </summary>
public class MultiworldMenu : ModMenu
{
    private readonly ServerConnection _connection;
    private readonly MenuFramework _menuMod;

    private object _resultCoroutine = null;

    /// <summary>
    /// Maximum priority
    /// </summary>
    protected override int Priority { get; } = int.MaxValue;

    /// <summary>
    /// Initialize the menu connection
    /// </summary>
    public MultiworldMenu(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnConnect += OnConnect;
        _menuMod = (MenuFramework)ModHelper.GetModById("BlasII.Framework.Menus");
    }

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

        _resultText = UIModder.Create(new RectCreationOptions()
        {
            Name = "resulttext",
            Parent = ui,
            Position = new Vector2(0, -280),
        }).AddText(new TextCreationOptions()
        {
            FontSize = 56,
        }).AddShadow();
    }

    /// <summary>
    /// Restore client settings to menu
    /// </summary>
    public override void OnStart()
    {
        ConnectionInfo info = _connection.ConnectionInfo;
        ModLog.Info($"Starting menu with {info?.ToString() ?? "nothing"}");

        _setServer.CurrentValue = info?.Server ?? string.Empty;
        _setName.CurrentValue = info?.Name ?? string.Empty;
        _setPassword.CurrentValue = info?.Password ?? string.Empty;
        _resultText.SetText(string.Empty);
    }

    /// <summary>
    /// Overrides the 'enter' key to start connect process
    /// </summary>
    public override void OnUpdate()
    {
        if (Main.Multiworld.InputHandler.GetButtonDown(ButtonType.UIConfirm))
        {
            MelonCoroutines.Start(Connect());
        }
        else if (Main.Multiworld.InputHandler.GetButtonDown(ButtonType.UICancel))
        {
            _menuMod.ShowPreviousMenu();
        }
    }

    /// <summary>
    /// Store client settings from menu
    /// </summary>
    public override void OnFinish()
    {
        Multiworld.IGNORE_DATA_CLEAR = true;
    }

    private IEnumerator Connect()
    {
        DisplayResult(Main.Multiworld.LocalizationHandler.Localize("result/connect"), RESULT_INFO, 0);

        yield return null;
        yield return null;

        var info = new ConnectionInfo(_setServer.CurrentValue, _setName.CurrentValue, _setPassword.CurrentValue);
        _connection.Connect(info);
    }

    private void OnConnect(LoginResult result)
    {
        if (result is LoginFailure failure)
        {
            string text = $"{Main.Multiworld.LocalizationHandler.Localize("result/fail")} {string.Join(", ", failure.Errors)}";
            DisplayResult(text, RESULT_ERROR, 5);
            return;
        }

        if (result is LoginSuccessful)
        {
            _menuMod.ShowNextMenu();
            return;
        }
    }

    private void DisplayResult(string message, Color32 color, float time)
    {
        if (_resultCoroutine != null)
            MelonCoroutines.Stop(_resultCoroutine);

        _resultText.SetText(message);
        _resultText.SetColor(color);

        if (time > 0)
            _resultCoroutine = MelonCoroutines.Start(HideResult(time));
    }

    private IEnumerator HideResult(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _resultText.SetText(string.Empty);
    }

    private TextOption _setServer;
    private TextOption _setName;
    private TextOption _setPassword;
    private UIPixelTextWithShadow _resultText;

    private const int TEXT_SIZE = 56;
    private readonly Color32 SILVER = new Color32(192, 192, 192, 255);
    private readonly Color32 YELLOW = new Color32(255, 231, 65, 255);
    private readonly Color32 RESULT_INFO = new Color32(0, 107, 61, 255);
    private readonly Color32 RESULT_ERROR = new Color32(214, 31, 31, 255);
}
