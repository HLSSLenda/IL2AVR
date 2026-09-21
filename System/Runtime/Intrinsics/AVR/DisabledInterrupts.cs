namespace System.Runtime.Intrinsics.AVR;

/// <summary>Temporarily disables interrupts.</summary>
public readonly ref struct DisabledInterrupts : IDisposable
{
    public DisabledInterrupts() => Interrupts.Disable();
    public void Dispose() => Interrupts.Enable();
}