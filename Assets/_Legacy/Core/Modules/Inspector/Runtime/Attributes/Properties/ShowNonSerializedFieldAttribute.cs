using System;

namespace Things
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ShowNonSerializedAttribute : Attribute { }
}
