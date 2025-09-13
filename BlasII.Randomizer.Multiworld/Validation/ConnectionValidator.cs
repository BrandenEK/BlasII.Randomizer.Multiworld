using Archipelago.MultiClient.Net;
using BlasII.ModdingAPI;
using Il2CppTGK.Game;
using System;

namespace BlasII.Randomizer.Multiworld.Validation;

/// <summary>
/// Ensures the game state is compatible with the server settings
/// </summary>
public class ConnectionValidator
{
    /// <summary>
    /// Checks the slotdata to determine if the connection should succeed
    /// </summary>
    public ValidationResult Validate(LoginResult result)
    {
        if (result is not LoginSuccessful success)
            throw new Exception("How did you get here with a login failure???");

        var worldVersion = new Version((string)success.SlotData["worldVersion"]);
        var requiredVersion = new Version(MIN_WORLD_VERSION);
        ModLog.Info($"Validating version with worldVersion={worldVersion} & requiredVersion={requiredVersion}");

        if (worldVersion < requiredVersion)
            return new ValidationResult(false, $"The apworld is out-of-date. Required version is {MIN_WORLD_VERSION}");

        bool dlcOwned = CoreCache.DLCManager.IsOwned(CoreCache.DLCManager.FindDLCByName("DLC01"));
        bool dlcRequired = true;
        ModLog.Info($"Validating dlc with dlcOwned={dlcOwned} & dlcRequired={dlcRequired}");

        if (!dlcOwned && dlcRequired)
            return new ValidationResult(false, $"The Mea Culpa dlc is required");

        return new ValidationResult(true, string.Empty);
    }

    private const string MIN_WORLD_VERSION = "1.1.0";
}
