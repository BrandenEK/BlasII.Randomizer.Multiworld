using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Models;
using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld.Services;

/// <summary>
/// Allows entering required AP info before the game starts
/// </summary>
public class MultiworldMenu : ModMenu
{
    private readonly ServerConnection _connection;

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

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (UnityEngine.Input.GetKeyDown(KeyCode.Delete))
        {
            StartConnectProcess();
        }
    }

    /// <summary>
    /// Store client settings from menu
    /// </summary>
    public override void OnFinish()
    {
        // Validate connected first


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
            // TODO: finish the menu
        }
    }

    private TextOption _setServer;
    private TextOption _setName;
    private TextOption _setPassword;

    private const int TEXT_SIZE = 56;
    private readonly Color32 SILVER = new Color32(192, 192, 192, 255);
    private readonly Color32 YELLOW = new Color32(255, 231, 65, 255);
}
