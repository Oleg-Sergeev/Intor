using Assets.Scripts.Utilities.Saving;

namespace Assets.Scripts.Data.SaveData
{
    public class ItemObjectData : ItemData
    {
        public bool IsDeleted { get; set; }


        public ItemObjectData(int id) : base(id)
        {
        }
    }
}
