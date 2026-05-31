using UnityEngine;
using UnityEngine.UI;

namespace Things
{
    [System.Serializable]
    public class TutorialBaseSave : ISaveObject
    {
        public bool isActive;
        public bool isFinished;

        public int progress;

        public void Flush()
        {

        }
    }
}