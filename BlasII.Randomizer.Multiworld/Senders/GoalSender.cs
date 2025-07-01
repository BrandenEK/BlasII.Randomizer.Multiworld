using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld.Senders;

/// <summary>
/// Handles sending a completed goal to the AP server
/// </summary>
public class GoalSender
{
    private readonly ServerConnection _connection;

    /// <summary>
    /// Initializes a new GoalSender
    /// </summary>
    public GoalSender(ServerConnection connection)
    {
        _connection = connection;
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
}
