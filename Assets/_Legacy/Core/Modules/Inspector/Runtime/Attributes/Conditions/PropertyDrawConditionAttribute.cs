using System;

namespace Things
{
    public class PropertyConditionAttribute : BaseAttribute
    {
        public PropertyConditionAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
