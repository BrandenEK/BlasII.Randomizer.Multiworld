using Archipelago.MultiClient.Net.Models;

namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Data for an item received from the AP server in the queue
/// </summary>
public class QueuedItemInfo(ItemInfo item, int index)
{
    /// <summary>
    /// Data for the actual received item
    /// </summary>
    public ItemInfo Item { get; } = item;

    /// <summary>
    /// THe received item index
    /// </summary>
    public int Index { get; } = index;
}
