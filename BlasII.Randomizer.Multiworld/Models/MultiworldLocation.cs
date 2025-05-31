
namespace BlasII.Randomizer.Multiworld.Models;

public class MultiworldLocation
{
    public string InternalId { get; }

    public long ServerId { get; }

    public MultiworldLocation(string Internal, long ap)
    {
        InternalId = Internal;
        ServerId = ap;
    }
}
