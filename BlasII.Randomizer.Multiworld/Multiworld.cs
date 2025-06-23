using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Persistence;
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
public class Multiworld : BlasIIMod, ISlotPersistentMod<MultiworldSlotData>
{
    // Hopefully dont need this in the future
    private readonly ServerConnection _connection = new();

    private readonly LocationSender _locationSender;

    private readonly ErrorReceiver _errorReceiver;
    private readonly ItemReceiver _itemReceiver;

    /// <summary>
    /// The current connection details
    /// </summary>
    public ConnectionInfo CurrentConnection { get; set; } = null;

    internal Multiworld() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _locationSender = new LocationSender(_connection);

        _errorReceiver = new ErrorReceiver();
        _itemReceiver = new ItemReceiver(_connection);
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
        IGNORE_DATA_CLEAR = false;
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

    /// <summary>
    /// Saves the slot data
    /// </summary>
    public MultiworldSlotData SaveSlot()
    {
        return new MultiworldSlotData()
        {
            connection = CurrentConnection,
            itemsReceived = _itemReceiver.ItemsReceived,
        };
    }

    /// <summary>
    /// Loads the slot data
    /// </summary>
    public void LoadSlot(MultiworldSlotData data)
    {
        // It resets/loads data after finishing a menu, so skip until the next scene load
        if (IGNORE_DATA_CLEAR)
            return;

        CurrentConnection = data.connection;
        _itemReceiver.ItemsReceived = data.itemsReceived;
    }

    /// <summary>
    /// Reset all slot data
    /// </summary>
    public void ResetSlot()
    {
        // It resets/loads data after finishing a menu, so skip until the next scene load
        if (IGNORE_DATA_CLEAR)
            return;

        CurrentConnection = null;
        _itemReceiver.ItemsReceived = 0;
    }

    /// <summary>
    /// Attempts to connect to the AP server
    /// </summary>
    public void Connect(ConnectionInfo info)
    {
        ArchipelagoSession session;
        LoginResult result;
        ModLog.Info($"Attempting with {info}");

        try
        {
            session = ArchipelagoSessionFactory.CreateSession(info.Server);
            session.Socket.ErrorReceived += _errorReceiver.OnReceiveError;
            session.Items.ItemReceived += _itemReceiver.OnReceiveItem;
            _connection.UpdateSession(session);

            result = session.TryConnectAndLogin("Blasphemous 2", info.Name, ItemsHandlingFlags.AllItems, new Version(0, 6, 0), null, null, info.Password, true);
        }
        catch (Exception ex)
        {
            result = new LoginFailure(ex.ToString());
            CurrentConnection = null;

            // temp
            ModLog.Warn(string.Join(", ", ((LoginFailure)result).Errors));
            return;
        }

        bool connected = result.Successful;
        ModLog.Info("Connection result: " + connected);
        _connection.InvokeConnect(result);
        CurrentConnection = info;

        // Should I not return here if not successful ???

        // parse slot data
        LoginSuccessful success = result as LoginSuccessful;

        // Load settings from slotdata
        RandomizerSettings settings = ((JObject)success.SlotData["settings"]).ToObject<RandomizerSettings>();
        settings.Seed = CalculateMultiworldSeed(session.RoomState.Seed, info.Name);
    }

    private int CalculateMultiworldSeed(string seed, string name)
    {
        return Math.Abs(((seed.GetHashCode() / 2) + (name.GetHashCode() / 2)) % RandomizerSettings.MAX_SEED);
    }

    internal static bool IGNORE_DATA_CLEAR = false;
}
