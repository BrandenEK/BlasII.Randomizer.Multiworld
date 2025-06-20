
namespace BlasII.Randomizer.Multiworld.Models;

/// <summary>
/// Models a mapping between the internal and server ids
/// </summary>
public class MultiworldLocation(string i, long s)
{
    /// <summary>
    /// The in-game location id
    /// </summary>
    public string InternalId { get; } = i;

    /// <summary>
    /// The AP server location id
    /// </summary>
    public long ServerId { get; } = s;
}
