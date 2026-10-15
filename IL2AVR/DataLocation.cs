namespace IL2AVR;

/// <summary>Specifies where data can be located in the program.</summary>
public enum DataLocation
{
    /// <summary>Specifies that the location is unknown/unspecified.</summary>
    Unknown,
    /// <summary>Specifies that the location is the program flash.</summary>
    Flash,
    /// <summary>Specifies that the location is in the static RAM.</summary>
    SRAM
}
