
namespace BlasII.Randomizer.Multiworld.Validation;

/// <summary>
/// The result from a validation attempt
/// </summary>
public class ValidationResult(bool isValid, string message)
{
    /// <summary>
    /// Whether the attempt is valid or not
    /// </summary>
    public bool IsValid { get; } = isValid;

    /// <summary>
    /// The error message, if it is invalid
    /// </summary>
    public string Message { get; } = message;
}
