using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class MoveSettings
    {
        [field: SerializeField]
        [field: Range(0, 100f)]
        public float TurningSpeed { get; set; } = 15f;

        [field: SerializeField]
        [field: Range(0, 100f)]
        public float MovementSmoothing { get; set; } = 5;

        [field: SerializeField]
        [field: Range(0, 100f)]
        public float MovementSpeed { get; set; } = 5;
    }
}