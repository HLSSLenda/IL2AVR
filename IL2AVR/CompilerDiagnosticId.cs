namespace IL2AVR;

public enum CompilerDiagnosticId
{
#region Fatals
    FUhOh,
#endregion
#region Errors
    EInvalidUnsafeAccessor,
    ENoEntryPoint,
    ENoMethodImplementation,
    EPointerOriginMismatch,
    ETypeAsWithInterface,
    EUnsupportedProcessorOpCode,
#endregion
#region Warnings
    WTypeAsWithAbstractClass,
#endregion
#region Informations
    ITypeAsWithReferenceTypes,
#endregion
#region Debugs
    DDebug
#endregion
}
