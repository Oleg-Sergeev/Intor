using System;
using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public abstract class PositionData : ItemData
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }


        public PositionData(string id) : base(id)
        {
        }
    }
}
