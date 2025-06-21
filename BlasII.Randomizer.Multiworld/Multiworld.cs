using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Models;
using BlasII.Randomizer.Multiworld.Receivers;
using BlasII.Randomizer.Multiworld.Senders;
using BlasII.Randomizer.Multiworld.Services;
using Newtonsoft.Json.Linq;
using System;

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
    private readonly ItemReceiver _itemReceiver;

    internal Multiworld() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _locationSender = new LocationSender(_connection);

        _errorReceiver = new ErrorReceiver();
        _itemReceiver = new ItemReceiver();
    }

    /// <summary>
    /// Setup handlers on game start
    /// </summary>
    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        MessageHandler.AllowReceivingBroadcasts = true;
        MessageHandler.AddMessageListener("BlasII.Randomizer", "LOCATION", OnCheckLocation);
    }

    /// <summary>
    /// Registers all required services
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        var menu = new MultiworldMenu();
        provider.RegisterNewGameMenu(menu);
        provider.RegisterLoadGameMenu(menu);
    }

    /// <summary>
    /// Simulates connecting to AP
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        //if (!_connection.Connected)
        //    Connect("localhost", "B", null);
    }

    /// <summary>
    /// Simulates recieving items on keypress
    /// </summary>
    protected override void OnUpdate()
    {
        _itemReceiver.OnUpdate();
    }

    private void OnCheckLocation(string locationId)
    {
        ModLog.Warn($"Sending location {locationId}");
        ItemLocation location = Main.Randomizer.ItemLocationStorage[locationId];
        _locationSender.Send(location);
    }

    private void Connect(string server, string player, string password)
    {
        ArchipelagoSession session;
        LoginResult result;
        ModLog.Info($"Attempting to connect to {server} as {player} with password '{password}'");

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(server);
            session.Socket.ErrorReceived += _errorReceiver.OnReceiveError;
            session.Items.ItemReceived += _itemReceiver.OnReceiveItem;
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
