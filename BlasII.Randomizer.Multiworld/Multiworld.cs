using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using BlasII.Randomizer.Multiworld.Receivers;
using BlasII.Randomizer.Multiworld.Senders;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace BlasII.Randomizer.Multiworld;

/// <summary>
/// A multiworld client that allows the Randomizer to connect to AP
/// </summary>
public class Multiworld : BlasIIMod
{
    // Hopefully dont need this in the future
    private readonly ServerConnection _connection = new();

    private readonly LocationSender _locationSender;

    private readonly ErrorReceiver _errorReceiver;

    internal Multiworld() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _locationSender = new LocationSender(_connection);

        _errorReceiver = new ErrorReceiver();
    }

    /// <summary>
    /// Setup handlers on game start
    /// </summary>
    protected override void OnInitialize()
    {
        MessageHandler.AllowReceivingBroadcasts = true;
        MessageHandler.AddMessageListener("BlasII.Randomizer", "LOCATION", (content) =>
        {
            ModLog.Warn("Multiworld will send out location id: " + content);
            ItemLocation location = Main.Randomizer.ItemLocationStorage[content];
            _locationSender.Send(location);
        });
    }

    /// <summary>
    /// Simulates connecting to AP
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        if (!_connection.Connected)
            Connect("localhost", "B", null);
    }

    /// <summary>
    /// Simulates recieving items on keypress
    /// </summary>
    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ModLog.Warn("Multiworld will receive item id: " + "TEST_ID");
        }
    }

    private void Connect(string server, string player, string password)
    {
        ArchipelagoSession session;
        LoginResult result;
        ModLog.Info($"Attempting to connect to {server} as {player} with password '{password}'");

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(server);
            session.Socket.ErrorReceived += _errorReceiver.Handle;
            _connection.UpdateSession(session);

            result = session.TryConnectAndLogin("Blasphemous 2", player, ItemsHandlingFlags.AllItems, new Version(0, 6, 0), null, null, password, true);
        }
        catch (Exception ex)
        {
            result = new LoginFailure(ex.ToString());

            // temp
            ModLog.Warn(string.Join(", ", ((LoginFailure)result).Errors));
            return;
        }

        bool connected = result.Successful;
        ModLog.Info("Connection result: " + connected);
        _connection.InvokeConnect(result);

        // parse slot data
        LoginSuccessful success = result as LoginSuccessful;

        // Load settings from slotdata
        RandomizerSettings settings = ((JObject)success.SlotData["settings"]).ToObject<RandomizerSettings>();
        settings.Seed = CalculateMultiworldSeed(session.RoomState.Seed, player);
    }

    private int CalculateMultiworldSeed(string seed, string name)
    {
        return Math.Abs(((seed.GetHashCode() / 2) + (name.GetHashCode() / 2)) % RandomizerSettings.MAX_SEED);
    }
}
