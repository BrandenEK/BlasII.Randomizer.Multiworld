using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BlasII.ModdingAPI;
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
    public bool Connected => Session is not null && Session.Socket.Connected;

    /// <summary>
    /// Attempts to connect to the AP server
    /// </summary>
    public void Connect(ConnectionInfo info)
    {
        LoginResult result;
        ModLog.Info($"Calling connect with {info}");

        try
        {
            Session = ArchipelagoSessionFactory.CreateSession(info.Server);
            Main.Multiworld.SetReceiverCallbacks(Session);
            Session.Socket.SocketClosed += (_) => OnDisconnect?.Invoke();

            result = Session.TryConnectAndLogin("Blasphemous 2", info.Name, ItemsHandlingFlags.AllItems, new Version(0, 6, 0), null, null, info.Password, true);
        }
        catch (Exception ex)
        {
            var failure = new LoginFailure(ex.ToString());
            ModLog.Error($"Error on connection: {string.Join(", ", failure.Errors)}");

            result = failure;
        }

        ModLog.Info("Connection result: " + result.Successful);
        ConnectionInfo = info;
        OnConnect?.Invoke(result);
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
