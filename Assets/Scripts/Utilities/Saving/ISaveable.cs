namespace Assets.Scripts.Utilities.Saving
{
    public interface ISaveable
    {
        string Id { get; }


        void SetItemData(ItemData itemData);

        ItemData GetItemData();
    }
}
