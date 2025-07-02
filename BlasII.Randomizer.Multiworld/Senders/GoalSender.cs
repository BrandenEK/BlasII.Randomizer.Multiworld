using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using BlasII.ModdingAPI;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Senders;

/// <summary>
/// Handles sending a completed goal to the AP server
/// </summary>
public class GoalSender
{
    private readonly ServerConnection _connection;

    private int _chosenEnding;

    /// <summary>
    /// Initializes a new GoalSender
    /// </summary>
    public GoalSender(ServerConnection connection)
    {
        _connection = connection;
        _connection.OnConnect += OnConnect;
    }

    private void OnConnect(LoginResult result)
    {
        _chosenEnding = 0;

        if (result is not LoginSuccessful success)
            return;

        _chosenEnding = int.Parse(success.SlotData["ending"].ToString());
        ModLog.Info($"Received chosen ending ({_chosenEnding}) from slot data");
    }

    /// <summary>
    /// Sends a goal packet
    /// </summary>
    public void SendGoal()
    {
        if (!_connection.Connected)
            return;

        var packet = new StatusUpdatePacket();
        packet.Status = ArchipelagoClientState.ClientGoal;
        _connection.Session.Socket.SendPacket(packet);
    }

    /// <summary>
    /// Checks the completed ending and potentially sends a goal packet
    /// </summary>
    public void CheckAndSendGoal(int ending)
    {
        ModLog.Info($"Checking completed goal ({ending}) against chosen goal ({_chosenEnding})");

        if (ending >= _chosenEnding)
            SendGoal();
    }
}
