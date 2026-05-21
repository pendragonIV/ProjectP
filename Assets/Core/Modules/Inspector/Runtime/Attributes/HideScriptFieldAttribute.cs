using System;

namespace ProjectP
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HideScriptFieldAttribute : Attribute { }
}
