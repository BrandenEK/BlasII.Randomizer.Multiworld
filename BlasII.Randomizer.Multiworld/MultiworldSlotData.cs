using BlasII.ModdingAPI.Persistence;
using BlasII.Randomizer.Multiworld.Models;

namespace BlasII.Randomizer.Multiworld;

/// <summary>
/// Stores save data for the Multiworld client
/// </summary>
public class MultiworldSlotData : SlotSaveData
{
    /// <summary>
    /// The connection details for the server room
    /// </summary>
    public ConnectionInfo connection;

    /// <summary>
    /// The current number of received items
    /// </summary>
    public int itemsReceived;
}
