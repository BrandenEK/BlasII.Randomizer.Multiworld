using Archipelago.MultiClient.Net;
using BlasII.ModdingAPI;
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
        ModLog.Info($"Calling validate with worldVersion={worldVersion} & requiredVersion={requiredVersion}");

        if (worldVersion < requiredVersion)
            return new ValidationResult(false, $"The apworld is out-of-date. Required version is {MIN_WORLD_VERSION}");

        return new ValidationResult(true, string.Empty);
    }

    private const string MIN_WORLD_VERSION = "1.0.2";
}
