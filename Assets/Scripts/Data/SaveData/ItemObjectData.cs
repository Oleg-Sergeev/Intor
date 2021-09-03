using System;
using Assets.Scripts.Utilities.Saving;

namespace Assets.Scripts.Data.SaveData
{
    [Serializable]
    public class ItemObjectData : ItemData
    {
        public bool IsDeleted { get; set; }


        public ItemObjectData(string id) : base(id)
        {
        }
    }
}
