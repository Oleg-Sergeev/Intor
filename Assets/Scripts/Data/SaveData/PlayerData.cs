namespace Assets.Scripts.Data.SaveData
{
    public class PlayerData : PositionData
    {
        public Inventory Inventory { get; set; }


        public PlayerData(int id) : base(id)
        {
        }
    }
}
