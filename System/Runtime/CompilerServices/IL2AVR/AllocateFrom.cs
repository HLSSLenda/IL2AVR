namespace System.Runtime.CompilerServices.IL2AVR;

[AttributeUsage(AttributeTargets.Parameter)]
#pragma warning disable CS9113
/// <summary>Tells the IL2AVR compiler that a class field should be allocated by the size of a specific parameter.</summary>
/// <param name="at">The class field name to syntetize an extra allocation.</param>
/// <param name="property">The property/field of the parameter to pick the size from.</param>
/// <remarks>
/// <para>If this attribute is not used inside a constructor definition, IL2AVR emits an error.</para>
/// <para>If the field referenced from <paramref name="at" /> doesn't exist in the class, IL2AVR emits an error.</para>
/// <para>If the property referenced from <paramref name="property" /> doesn't exist in the parameter type, IL2AVR emits an error.</para>
/// <para>
/// This attribute doesn't change <see cref="System.Runtime.CompilerServices.Unsafe.SizeOf{T}" />'s output because this attribute
/// is intended for the compiler to allocate the right amount of <b>memory</b> on a constructor call. A raw allocation shall involve
/// calculating this size manually yourself, or providing a way to get the right size.
/// IL2AVR will emit a warning though in a usage of <see cref="System.Runtime.CompilerServices.Unsafe.SizeOf{T}" /> with a class that's annotated with this type.
/// </para>
/// </remarks>
public sealed class AllocateFrom(string at, string? property = null) : Attribute;
#pragma warning restore CS9113