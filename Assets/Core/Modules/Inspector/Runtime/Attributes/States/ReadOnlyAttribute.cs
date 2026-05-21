using System;

namespace ProjectP
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : Attribute
    {
        public ReadOnlyAttribute() { }
    }
}
