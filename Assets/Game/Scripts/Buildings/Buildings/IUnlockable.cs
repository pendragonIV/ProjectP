using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectP
{
    public interface IUnlockable
    {
        void SpawnUnlocked();
        void SpanwNotUnlocked();

        void FullyUnlock();

        public void SetID(string id);
    }
}