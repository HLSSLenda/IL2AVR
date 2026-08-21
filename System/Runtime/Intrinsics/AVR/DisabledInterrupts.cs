namespace System.Runtime.Intrinsics.AVR;

public readonly ref struct DisabledInterrupts : IDisposable
{
    public DisabledInterrupts() => Interrupts.Disable();
    public void Dispose() => Interrupts.Enable();
}