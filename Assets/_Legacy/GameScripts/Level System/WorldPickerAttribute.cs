using System;
using UnityEngine;

namespace Things
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class WorldPickerAttribute : PropertyAttribute
    {

    }
}
