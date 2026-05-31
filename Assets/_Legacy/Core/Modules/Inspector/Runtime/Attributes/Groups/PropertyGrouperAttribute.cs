using System;

namespace Things
{
    public class PropertyGrouperAttribute : BaseAttribute
    {
        public PropertyGrouperAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
