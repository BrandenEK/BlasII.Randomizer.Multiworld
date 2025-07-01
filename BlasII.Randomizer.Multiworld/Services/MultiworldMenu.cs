using Archipelago.MultiClient.Net;
using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using BlasII.Framework.UI;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Helpers;
using BlasII.Randomizer.Multiworld.Models;
using Il2CppTMPro;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Services;

/// <summary>
/// Allows entering required AP info before the game starts
/// </summary>
public class MultiworldMenu : ModMenu
{
    private readonly ServerConnection _connection;
    private readonly MenuFramework _menuMod;

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

        _resultText = UIModder.Create()
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
    }

    /// <summary>
    /// Overrides the 'enter' key to start connect process
    /// </summary>
    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartConnectProcess();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
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

    private void StartConnectProcess()
    {
        var info = new ConnectionInfo(_setServer.CurrentValue, _setName.CurrentValue, _setPassword.CurrentValue);
        _connection.Connect(info);
    }

    private void OnConnect(LoginResult result)
    {
        if (result is LoginFailure failure)
        {
            // TODO: Display failure in menu
            ModLog.Error("Faulure;");
            return;
        }

        if (result is LoginSuccessful success)
        {
            _menuMod.ShowNextMenu();
            return;
        }
    }

    private TextOption _setServer;
    private TextOption _setName;
    private TextOption _setPassword;
    private TMP_Text _resultText;

    private const int TEXT_SIZE = 56;
    private readonly Color32 SILVER = new Color32(192, 192, 192, 255);
    private readonly Color32 YELLOW = new Color32(255, 231, 65, 255);
}
