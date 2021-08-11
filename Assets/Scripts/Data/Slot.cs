using Assets.Scripts.Data.Items;

namespace Assets.Scripts.Data
{
    public class Slot
    {
        public Item Item { get; set; }

        public int Amount { get; set; }


        public Slot(Item item, int amount = 1)
        {
            Item = item;
            Amount = amount;
        }
    }
}
