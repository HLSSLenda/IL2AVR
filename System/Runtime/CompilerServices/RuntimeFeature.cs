namespace System.Runtime.CompilerServices;

public static class RuntimeFeature
{
    public const string ByRefFields = nameof(ByRefFields),
                        ByRefLikeGenerics = nameof(ByRefLikeGenerics),
                        DefaultImplementationsOfInterfaces = nameof(DefaultImplementationsOfInterfaces),
                        UnmanagedSignatureCallingConvention = nameof(UnmanagedSignatureCallingConvention),
                        VirtualStaticsInInterfaces = nameof(VirtualStaticsInInterfaces);
}