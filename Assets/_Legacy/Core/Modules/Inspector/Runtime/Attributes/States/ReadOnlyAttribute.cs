using System;

namespace Things
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : Attribute
    {
        public ReadOnlyAttribute() { }
    }
}
