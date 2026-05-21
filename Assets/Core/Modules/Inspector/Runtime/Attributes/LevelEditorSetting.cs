using System;

namespace ProjectP
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class LevelEditorSetting : Attribute
    {
        public LevelEditorSetting()
        {

        }
    }
}
