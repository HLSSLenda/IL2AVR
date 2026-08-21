using System.Runtime.CompilerServices.IL2AVR;
namespace System.Threading;

public static class Thread
{
    public static void Sleep([TypeAs(typeof(uint))] int ms)
    {
        // TODO
        Assembly.Asm(OpCodeId.Ret);
    }
}
