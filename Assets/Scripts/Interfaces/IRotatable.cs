using System;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IRotatable
    {
        void Rotate(Quaternion rotation, Action callback = default, float? speed = null);
    }
}
