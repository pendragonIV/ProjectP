using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectP
{
    public interface IUpgradePanel
    {
        GameObject UpgradeUIPrefab { get; }
        Transform ContentTransform { get; }
        bool ShowAllAfterUpgrade { get; set;  } 
        Color DefaultColor { get; }
        Color HighlightedColor { get; }
    }
}