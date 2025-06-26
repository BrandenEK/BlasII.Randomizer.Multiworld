using BlasII.ModdingAPI;
using BlasII.ModdingAPI.Files;
using System.Linq;

namespace BlasII.Randomizer.Multiworld.Storages;

/// <summary>
/// Stores and maps server/internal ids
/// </summary>
public class IdStorage
{
    private readonly MWID[] _ids;

    /// <summary>
    /// Loads all required ids
    /// </summary>
    public IdStorage(FileHandler handler, string filename)
    {
        handler.LoadDataAsJson(filename, out _ids);
        ModLog.Info($"Loaded {_ids.Length} id mappings from {filename}");
    }

    /// <summary>
    /// Converts a server id to an internal id
    /// </summary>
    public string ServerToInternalId(long id)
    {
        return _ids.First(x => x.ServerId == id).InternalId;
    }

    /// <summary>
    /// Converts an internal id to a server id
    /// </summary>
    public long InternalToServerId(string id)
    {
        return _ids.First(x => x.InternalId == id).ServerId;
    }

    class MWID(string i, long s)
    {
        public string InternalId { get; } = i;

        public long ServerId { get; } = s;
    }
}
