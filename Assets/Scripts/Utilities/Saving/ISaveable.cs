namespace Assets.Scripts.Utilities.Saving
{
    public interface ISaveable
    {
        int Id { get; }


        void SetItemData(ItemData itemData);

        ItemData GetItemData();
    }
}
