using System;
using UnityEngine;

namespace Assets.Scripts.Data.Puzzles.Doors
{
    [Serializable]
    public class DoorMechanicalSettings : DoorBaseSettings
    {
        [field: SerializeField]
        public int PuzzleTime { get; private set; }

        [field: SerializeField]
        public int HandleWidth { get; private set; }

        [field: SerializeField]
        public int PointSpeed { get; private set; }

        [field: SerializeField]
        public float PointSmoothness { get; private set; }

        [field: SerializeField]
        public float HandleReverseTime { get; private set; }

        [field: SerializeField]
        public float HandleReverseAcceleration { get; private set; }

        [field: SerializeField]
        public float HandleReverseSpeed { get; private set; }

        [field: SerializeField]
        public MovementFunc HandleMovementFunc { get; private set; }


        [Serializable]
        public class MovementFunc : SerializableCallback<float, float> { }
    }
}
