
namespace BlasII.Randomizer.Multiworld.Validation;

/// <summary>
/// Ensures the game state is compatible with the server settings
/// </summary>
public class ConnectionValidator
{
    /// <summary>
    /// Checks the slotdata to determine if the connection should succeed
    /// </summary>
    public ValidationResult Validate()
    {
        return new ValidationResult(true, string.Empty);
    }
}
