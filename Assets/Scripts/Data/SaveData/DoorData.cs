using System;
using Assets.Scripts.Utilities.Saving;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class DoorData : ItemData
    {
        public int State { get; set; }


        public DoorData(int id) : base(id)
        {
        }
    }
}
