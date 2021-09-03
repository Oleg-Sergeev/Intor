using System;

namespace Assets.Scripts.Utilities.Saving
{
    [Serializable]
    public abstract class ItemData
    {
        public string Id { get; }


        public ItemData(string id)
        {
            Id = id;
        }
    }
}
