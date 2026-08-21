namespace System.Reflection;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = false)]
public sealed class DefaultMemberAttribute(string memberName) : Attribute
{
    public readonly string MemberName = memberName;
}