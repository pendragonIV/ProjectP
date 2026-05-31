using System;

namespace Things
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HideScriptFieldAttribute : Attribute { }
}
