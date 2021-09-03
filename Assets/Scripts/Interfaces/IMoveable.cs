using System;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface IMoveable
    {
        void Move(Vector3 direction, Action callback = default, float? speed = null);
    }
}
