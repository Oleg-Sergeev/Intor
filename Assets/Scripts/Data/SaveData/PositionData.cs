using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Data.SaveData
{
    public abstract class PositionData : ItemData
    {
        public Vector3 Position { get; set; }

        public Vector3 Rotation { get; set; }


        public PositionData(int id) : base(id)
        {
        }
    }
}
