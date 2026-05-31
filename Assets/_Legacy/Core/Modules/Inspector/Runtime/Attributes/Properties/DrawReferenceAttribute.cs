using System;
using UnityEngine;

namespace Things
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class DrawReferenceAttribute : PropertyAttribute
    {

    }
}