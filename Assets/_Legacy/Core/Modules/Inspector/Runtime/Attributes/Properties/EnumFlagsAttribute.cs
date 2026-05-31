using System;

namespace Things
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnumFlagsAttribute : Attribute
    {
        public EnumFlagsAttribute()
        {

        }
    }
}
