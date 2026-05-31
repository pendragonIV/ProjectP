using System;

namespace Things
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LevelEditorSetting : Attribute
    {
        public LevelEditorSetting()
        {

        }
    }
}
