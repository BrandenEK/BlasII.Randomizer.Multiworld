using Archipelago.MultiClient.Net;
using BlasII.Randomizer.Multiworld.Models;
using Newtonsoft.Json.Linq;
using System;

namespace BlasII.Randomizer.Multiworld.Receivers;

/// <summary>
/// Handles receiving settings from the AP server
/// </summary>
public class SettingsReceiver
{
    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes a new SettingsReceiver
    /// </summary>
    public SettingsReceiver(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnConnect += OnConnect;
    }

    private void OnConnect(LoginResult result)
    {
        if (result is not LoginSuccessful success)
            return;

        // Load settings from slotdata
        RandomizerSettings settings = ((JObject)success.SlotData["settings"]).ToObject<RandomizerSettings>();
        settings.Seed = CalculateMultiworldSeed(_connection.Session.RoomState.Seed, _connection.ConnectionInfo.Name);

        Main.Randomizer.CurrentSettings = settings;
    }

    private int CalculateMultiworldSeed(string seed, string name)
    {
        return Math.Abs(((seed.GetHashCode() / 2) + (name.GetHashCode() / 2)) % RandomizerSettings.MAX_SEED);
    }
}
