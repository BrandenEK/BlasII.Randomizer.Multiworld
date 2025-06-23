using Archipelago.MultiClient.Net.Models;

namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Data for an item received from the AP server in the queue
/// </summary>
public class QueuedItemInfo(ItemInfo info, int index)
{
    /// <summary>
    /// The name of the item
    /// </summary>
    public string ItemName { get; } = info.ItemDisplayName;

    /// <summary>
    /// The player who found it
    /// </summary>
    public string PlayerName { get; } = info.Player.Name;

    /// <summary>
    /// THe received item index
    /// </summary>
    public int Index { get; } = index;
}
