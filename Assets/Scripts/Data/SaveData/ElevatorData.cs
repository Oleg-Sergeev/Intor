using Assets.Scripts.Utilities.Saving;
using UnityEngine;

namespace Assets.Scripts.Data.SaveData
{
    public class ElevatorData : ItemData
    {
        public bool IsWorking { get; set; }

        public Vector3 Position { get; set; }


        public ElevatorData(int id) : base(id)
        {
        }
    }
}
