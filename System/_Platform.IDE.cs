#if !ARDUINO
namespace System
{
    namespace Runtime.CompilerServices.IL2AVR
    {
        public partial interface INativeMemoryAllocator
        {
            public const int RAMSTART = 0, RAMEND = 0, SPH = 0, SPL = 0;
        }
    }
}
#endif