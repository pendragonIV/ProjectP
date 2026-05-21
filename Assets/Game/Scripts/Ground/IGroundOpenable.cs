using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectP
{
    public interface IGroundOpenable
    {
        void OnGroundOpen(bool immediately = false);
        void OnGroundHidden(bool immediately = false);
    }
}