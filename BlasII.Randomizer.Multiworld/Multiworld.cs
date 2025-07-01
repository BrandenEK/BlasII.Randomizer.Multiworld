using Archipelago.MultiClient.Net;
using BlasII.Framework.Menus;
using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Persistence;
using BlasII.Randomizer.Models;
using BlasII.Randomizer.Multiworld.Displays;
using BlasII.Randomizer.Multiworld.Models;
using BlasII.Randomizer.Multiworld.Receivers;
using BlasII.Randomizer.Multiworld.Senders;
using BlasII.Randomizer.Multiworld.Services;
using BlasII.Randomizer.Multiworld.Storages;
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
    private readonly SettingsReceiver _settingsReceiver;

    private readonly StatusDisplay _statusDisplay;

    internal IconStorage IconStorage { get; private set; }
    internal IdStorage ItemStorage { get; private set; }
    internal IdStorage LocationStorage { get; private set; }

    internal Scouter Scouter { get; private set; }

    internal Multiworld() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
        _locationSender = new LocationSender(_connection);

        _errorReceiver = new ErrorReceiver();
        _itemReceiver = new ItemReceiver(_connection);
        _settingsReceiver = new SettingsReceiver(_connection);

        _statusDisplay = new StatusDisplay(_connection);

        // TODO: Move to a separate class
        _connection.OnDisconnect += () => ModLog.Warn("Disconnected from server!!");
    }

    /// <summary>
    /// Setup handlers on game start
    /// </summary>
    protected override void OnInitialize()
    {
        LocalizationHandler.RegisterDefaultLanguage("en");
        MessageHandler.AllowReceivingBroadcasts = true;
        MessageHandler.AddMessageListener("BlasII.Randomizer", "LOCATION", OnCheckLocation);

        IconStorage = new IconStorage(FileHandler);
        ItemStorage = new IdStorage(FileHandler, "itemids.json");
        LocationStorage = new IdStorage(FileHandler, "locationids.json");

        Scouter = new Scouter(_connection);
    }

    /// <summary>
    /// Registers all required services
    /// </summary>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        var menu = new MultiworldMenu(_connection);
        provider.RegisterNewGameMenu(menu);
        provider.RegisterLoadGameMenu(menu);
    }

    /// <summary>
    /// Resets the loading flag
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        IGNORE_DATA_CLEAR = false;
    }

    /// <summary>
    /// Updates the required components
    /// </summary>
    protected override void OnUpdate()
    {
        _itemReceiver.OnUpdate();
        _statusDisplay.OnUpdate();
    }

    /// <summary>
    /// Handles disconnect when exiting the game
    /// </summary>
    protected override void OnExitGame()
    {
        _connection.Disconnect();
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
            connection = _connection.ConnectionInfo,
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

        _connection.ConnectionInfo = data.connection;
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

        _connection.ConnectionInfo = null;
        _itemReceiver.ItemsReceived = 0;
    }

    /// <summary>
    /// Adds the required receiver callbacks to the session
    /// </summary>
    public void SetReceiverCallbacks(ArchipelagoSession session)
    {
        session.Socket.ErrorReceived += _errorReceiver.OnReceiveError;
        session.Items.ItemReceived += _itemReceiver.OnReceiveItem;
    }

    internal static bool IGNORE_DATA_CLEAR = false;
}
