using System;

namespace ProjectP
{
    public static class UniqueIDUtils
    {
        public static string GetUniqueID()
        {
            Guid guid = Guid.NewGuid();

            return guid.ToString();
        }
    }
}
