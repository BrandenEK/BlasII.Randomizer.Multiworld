using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Validation;
using System;

namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Represents the ArchipelagoSession connection
/// </summary>
public class ServerConnection
{
    /// <summary> The session object </summary>
    public ArchipelagoSession Session { get; private set; } = ArchipelagoSessionFactory.CreateSession("localhost");

    /// <summary> The details of the current connection </summary>
    public ConnectionInfo ConnectionInfo { get; set; } = new ConnectionInfo(string.Empty, string.Empty, string.Empty);

    /// <summary> Whether the server is connected </summary>
    public bool Connected => Session.Socket.Connected;

    private readonly ConnectionValidator _validator = new();

    private bool _wasConnected;

    /// <summary>
    /// Attempts to connect to the AP server
    /// </summary>
    public void Connect(ConnectionInfo info)
    {
        LoginResult login;
        ModLog.Info($"Calling connect with {info}");

        try
        {
            Session = ArchipelagoSessionFactory.CreateSession(info.Server);
            Main.Multiworld.SetReceiverCallbacks(Session);

            login = Session.TryConnectAndLogin("Blasphemous 2", info.Name, ItemsHandlingFlags.AllItems, new Version(0, 6, 0), null, null, info.Password, true);
        }
        catch (Exception ex)
        {
            var failure = new LoginFailure(ex.ToString());
            ModLog.Error($"Error on connection: {string.Join(", ", failure.Errors)}");

            login = failure;
        }

        var result = _validator.Validate();

        if (!result.IsValid)
        {
            ModLog.Error("Something is wrong");
        }

        ModLog.Info($"Connection result: {login.Successful}");
        ModLog.Info($"Validation result: {result.IsValid}");

        ConnectionInfo = info;
        OnConnect?.Invoke(login);
    }

    /// <summary>
    /// Attempts to disconnect from the AP server
    /// </summary>
    public void Disconnect()
    {
        ModLog.Info("Calling disconnect");
        Session.Socket.DisconnectAsync();
    }

    /// <summary>
    /// Called every frame to check if 'Connected' has changed
    /// </summary>
    public void CheckStatus()
    {
        bool isConnected = Connected;
        if (_wasConnected && !isConnected)
        {
            OnDisconnect?.Invoke();
        }

        _wasConnected = isConnected;
    }

    /// <summary>
    /// Delegate for handling connect events
    /// </summary>
    public delegate void ConnectDelegate(LoginResult result);
    
    /// <summary>
    /// Called after connecting to the server
    /// </summary>
    public event ConnectDelegate OnConnect;

    /// <summary>
    /// Delegate for handling disconnect events
    /// </summary>
    public delegate void DisconnectDelegate();

    /// <summary>
    /// Called after disconnecting from the server
    /// </summary>
    public event DisconnectDelegate OnDisconnect;
}
