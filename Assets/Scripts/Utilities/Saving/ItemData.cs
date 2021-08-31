using System;

namespace Assets.Scripts.Utilities.Saving
{
    [Serializable]
    public abstract class ItemData
    {
        public int Id { get; }


        public ItemData(int id)
        {
            Id = id;
        }
    }
}
