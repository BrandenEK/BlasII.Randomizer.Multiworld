using Archipelago.MultiClient.Net;

namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Represents the ArchipelagoSession connection
/// </summary>
public class ServerConnection
{
    /// <summary> The session object </summary>
    public ArchipelagoSession Session { get; private set; }

    /// <summary> Whether the server is connected </summary>
    public bool Connected => Session is not null && Session.Socket.Connected;

    public delegate void ConnectDelegate(LoginResult result);
    //public delegate void DisconnectDelegate();

    public event ConnectDelegate OnConnect;
    //public event DisconnectDelegate OnDisconnect;

    /// <summary>
    /// Invokes the OnConnect event
    /// </summary>
    public void InvokeConnect(LoginResult result)
    {
        OnConnect?.Invoke(result);
    }

    /// <summary>
    /// Replaces the current session object with a new one
    /// </summary>
    public void UpdateSession(ArchipelagoSession session)
    {
        Session = session;
    }
}
